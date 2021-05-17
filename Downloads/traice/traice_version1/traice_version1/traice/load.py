import pandas as pd
import pickle
import os
from pathlib import Path

from traice import batchstep

TABLE_PICKLE_MAPPING = {
    'LEX_TRADE.csv' : 'trades.pkl',
    'LEX_ACCT_TRADE_BRIDGE.csv' : 'acct_trade_bridge.pkl',
    'LEX_ACCT.csv' : 'acct.pkl',
    'LEX_TRD_IND.csv' : 'trd_ind.pkl',
    'LEX_COMPLAINTS.csv' : 'complaints.pkl',
    'LEX_GROSS_IA_INCOME.csv' : 'gross_revs.pkl'
}

class Load(batchstep.BatchStep):

    def __init__(self, input_files, pickled_dir, completion_file):
        super().__init__(input_files, pickled_dir, completion_file)

    def run_step(self):
        
        pickle_dict_out = {}
        tables = []
        tables_idx = {}

        for file_name in self.input_files:
            df = pd.read_csv(file_name, encoding='latin1', low_memory=False)
            file_basename = os.path.basename(file_name)
            pickle_path = self.pickled_dir + '/' + TABLE_PICKLE_MAPPING[file_basename]
            pickle.dump(df, open(pickle_path, 'wb'))
    