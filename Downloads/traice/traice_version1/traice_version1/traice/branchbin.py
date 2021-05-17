import pandas as pd
import pickle
from pathlib import Path

from traice import batchstep

class BranchBin(batchstep.BatchStep):

    def __init__(self, input_dir, pickled_dir, out_path):

        super().__init__(input_dir, pickled_dir, out_path)

        # Inputs
        self.hit_list_expanded = None
        self.ia_kri_mapping = None

        # Outputs
        self.branch_binned = None
        self.joined_tables = None

    def risk_bin(self, x):
        '''
        Function to output binned risk as high, medium, or low based on risk score
        '''
        if x > 0.8:
            return 'H'
        elif x > 0.5:
            return 'M'
        else:
            return 'L'

    ## Branch Binned
    def run_step(self):
        
        self.hit_list_expanded = pickle.load(open(self.pickled_dir + '/hit_list_expanded.pkl', 'rb'))
        self.ia_kri_mapping = pickle.load(open(self.pickled_dir + '/ia_kri_mapping.pkl', 'rb'))

        # bin by branch and output to excel
        self.hit_list_expanded['risk_bin'] = self.hit_list_expanded['IA Risk Score'].apply(self.risk_bin)
        self.branch_binned = self.hit_list_expanded.groupby(['Branch #', 'risk_bin'])['IA ID'].count().reset_index()
        self.branch_binned[self.branch_binned['risk_bin'] == 'H']
        #branch_binned.to_excel(f'branch_binned_{this_run_id}.xlsx', index=False)
        self.branch_binned.to_excel(self.pickled_dir + '/branch_binned_02.xlsx', index=False)

        ## Joined tables
        """
        self.joined_tables = pd.merge(hit_list_expanded,
            ia_kri_mapping,
            left_on='IA ID',
            right_on='IA ID',how='left').to_excel(f'joined_tables_{this_run_id}.xlsx',index=False)
        """
        self.joined_tables = pd.merge(self.hit_list_expanded,
            self.ia_kri_mapping,
            left_on='IA ID',
            right_on='IA ID',how='left').to_excel(self.pickled_dir + '/joined_tables_02.xlsx', index=False)

        pickle.dump(self.branch_binned, open(self.pickled_dir + '/branch_binned.pkl', 'wb'))
        pickle.dump(self.joined_tables, open(self.pickled_dir + '/joined_tables.pkl', 'wb'))

