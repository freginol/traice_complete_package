import pandas as pd
import pickle
import random
import numpy as np
from pathlib import Path

from traice import batchstep

# TODO check that randomness is stabilized throughout code
class HitListExp(batchstep.BatchStep):

    def __init__(self, input_dir, pickled_dir, out_path):

        super().__init__(input_dir, pickled_dir, out_path)

        # Inputs
        self.hit_list = None
        self.gross_revs = None
        self.acct_trades = None

        # Outputs
        self.hit_list_out = None
        self.hit_list_expanded = None
        self.hit_list_expanded_out = None

    def sensible_decrease(self, x):
        '''
        Function to create increase/decrease values in risk score that are reasonable (i.e don't imply a
        risk score greater than 100 or less than 0)
        '''
        if 100-x < 10/100:
            return np.random.randint(x-100,10)/100
        elif x<10/100:
            return np.random.randint(-10,x)/100
        else:
            return np.random.randint(-10,10)/100

    # LOS = Length of Service
    def los_transform(self, x):
        '''
        Convert LOS from text/integer combination into integers only
        '''
        if x == 'OVER 10':
            return 10
        elif 'F' in x:
            return 0
        else:
            return int(x.strip())

    def personal_less_than_client(self, x):
        '''
        Correction for Personal ROA to be less than client ROA
        '''
        rand_val= np.random.random()*(0.25)+0.
        if x['Client ROA(%)'] < rand_val:
            return max(x['Client ROA(%)']-0.05,0)
        else:
            return rand_val

    ## Creating the expanded hit list
    def run_step(self):
        
        # TODO check this
        np.random.seed(123)

        self.hit_list = pickle.load(open(self.pickled_dir + '/hit_list.pkl', 'rb'))
        self.gross_revs = pickle.load(open(self.pickled_dir + '/gross_revs.pkl', 'rb'))
        self.acct_trades = pickle.load(open(self.pickled_dir + '/acct_trades.2.pkl', 'rb'))

        print(self.acct_trades.columns)

        # bridge table between IA and region
        ia_region_bridge = self.gross_revs[['FLATTEN_NAME','REGION']].drop_duplicates(['FLATTEN_NAME'])
        print(ia_region_bridge)
        # calculate annual gross revenues
        revs_annual = self.gross_revs.groupby('FLATTEN_NAME')['REV_CURMONTH'].sum().reset_index()
        revs_annual.columns = ['IA Name','REV_ANNUAL']

        # take original hit list and add in branch number and name
        self.hit_list_expanded = pd.merge(left=self.hit_list,right=self.acct_trades.drop_duplicates('IA_NAME')[['IA_NAME','RR_BRANCH_NUM','WM_PHY_BRANCH_NAME']],left_on='IA Name',right_on='IA_NAME')

        # add increase/decreaes to risk score
        self.hit_list_expanded['Increase/Decrease in Risk score(delta)'] = self.hit_list_expanded['IA Risk Score'].apply(self.sensible_decrease)

        # add region information
        self.hit_list_expanded = pd.merge(left=self.hit_list_expanded,right=ia_region_bridge,left_on='IA Name',right_on='FLATTEN_NAME')

        # add gross revenues
        self.hit_list_expanded = pd.merge(left=self.hit_list_expanded,right=revs_annual,on='IA Name',how='inner')

        # add minmax scaled gross revenues
        self.hit_list_expanded['REV_MinMax'] = (self.hit_list_expanded['REV_ANNUAL'] - self.hit_list_expanded['REV_ANNUAL'].min())/(self.hit_list_expanded['REV_ANNUAL'].max()-self.hit_list_expanded['REV_ANNUAL'].min())

        # calculate AUM using linear interpolation of gross revenues
        self.hit_list_expanded['AUM(in million $)'] = self.hit_list_expanded['REV_MinMax']*(1100-30)+30

        # calculate AUM growth from abs of normal distribution w mean 10 std 10
        self.hit_list_expanded['AUM growth %'] = self.hit_list_expanded['AUM(in million $)'].apply(lambda x: 0.10*abs(np.random.normal(0,1))*100)

        # get region rank
        self.hit_list_expanded['Rank #'] = self.hit_list_expanded.groupby(['REGION'])['REV_MinMax'].rank(ascending=False).astype(int).values

        # get number of accounts as proxy for households
        num_accts = self.acct_trades.groupby(['IA_NAME','ACCT_ID'])['TRD_TRADE_ID'].count().reset_index().groupby('IA_NAME')['TRD_TRADE_ID'].count().reset_index()
        num_accts.columns = ['IA_NAME','Num_Accts']
        self.hit_list_expanded = pd.merge(self.hit_list_expanded,num_accts,left_on='IA Name',right_on='IA_NAME',how='inner')
        self.hit_list_expanded['Households'] = self.hit_list_expanded['Num_Accts']

        # get number of IAs within region
        region_ia_counts = self.hit_list_expanded.groupby('REGION')['IA Name'].count().reset_index()
        region_ia_counts.columns = ['REGION','Total # of IAs within region']
        self.hit_list_expanded = pd.merge(self.hit_list_expanded,region_ia_counts,on='REGION',how='inner')

        # get LOS
        self.gross_revs['LOS(yrs)'] = self.gross_revs['LOS'].apply(self.los_transform)
        self.hit_list_expanded = pd.merge(self.hit_list_expanded,self.gross_revs[['FLATTEN_NAME','LOS(yrs)']].drop_duplicates('FLATTEN_NAME'),left_on='IA Name',right_on='FLATTEN_NAME')

        # get average number of trades
        avg_trades = self.acct_trades.groupby(['IA_NAME','TRD_MONTH'])['TRD_TRADE_ID'].count().reset_index().groupby('IA_NAME').mean().reset_index()
        avg_trades.columns = ['IA Name','Avg Trades(/month)']
        self.hit_list_expanded = pd.merge(self.hit_list_expanded,avg_trades,on='IA Name',how='left')

        # generate ROA and pro accounts values using random numbers, margin = 0
        self.hit_list_expanded['Client ROA(%)'] = self.hit_list_expanded['IA Name'].apply(lambda x: np.random.random()*(1.75)+0.)
        self.hit_list_expanded['Pro Acct(in million $)'] = self.hit_list_expanded['IA Name'].apply(lambda x: min(5,max(0.2,np.random.normal(0.5,1.))))
        self.hit_list_expanded['Margin(in $K)'] = 0
        self.hit_list_expanded['Personal ROA (in %)'] = self.hit_list_expanded['IA Name'].apply(lambda x: np.random.random()*(1.75)+0.)

        # add concatenation of branch number and name
        self.hit_list_expanded['Branch #'] = self.hit_list_expanded['RR_BRANCH_NUM'].astype(str) + '-' + self.hit_list_expanded['WM_PHY_BRANCH_NAME']

        # apply personal ROA correction to be less than client ROA
        self.hit_list_expanded['Personal ROA (in %)'] = self.hit_list_expanded.apply(self.personal_less_than_client,axis=1)

        ## Output desired columns for hit list and expanded hit list to excel

        hit_list_cols = ['IA ID','IA Name','IA Risk Score','S/M/L term','Increase/Decrease in Risk score(delta)',
                        'AUM(in million $)','AUM growth %']
        self.hit_list_out = self.hit_list_expanded[hit_list_cols]
        #self.hit_list_out.to_excel(f'hit_list_{this_run_id}.xlsx')
        self.hit_list_out.to_excel(self.pickled_dir + '/hit_list_02.xlsx')

        hit_list_expanded_cols = ['IA ID','Rank #','Total # of IAs within region',
            'Households','LOS(yrs)','Avg Trades(/month)','Client ROA(%)',
            'Pro Acct(in million $)', 'Margin(in $K)', 'Personal ROA (in %)',
            'Branch #']
        self.hit_list_expanded_out = self.hit_list_expanded[hit_list_expanded_cols]
        #self.hit_list_expanded_out.to_excel(f'hit_list_expanded_{this_run_id}.xlsx')
        self.hit_list_expanded_out.to_excel(self.pickled_dir + '/hit_list_expanded_02.xlsx')

        pickle.dump(self.hit_list_out, open(self.pickled_dir + '/hit_list_out.pkl', 'wb'))
        pickle.dump(self.hit_list_expanded, open(self.pickled_dir + '/hit_list_expanded.pkl', 'wb'))
        pickle.dump(self.hit_list_expanded_out, open(self.pickled_dir + '/hit_list_expanded_out.pkl', 'wb'))
