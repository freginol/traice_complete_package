import pandas as pd
import pickle
from pathlib import Path

from traice import batchstep
from traice import utils

class Aggregate(batchstep.BatchStep):

    def __init__(self, input_dir, pickled_dir, out_path):

        super().__init__(input_dir, pickled_dir, out_path)

        # Inputs
        self.acct_trades = None
        self.ia_branch_region_bridge = None
        self.flagged_extended = None
        self.dummy_cols = None
        self.code_dict = None
        self.pro_trades = None
        self.cancelled_trades = None
        self.complaints = None
        self.traded_under_other_ia = None
        self.acct_kyc_12m = None

        # Outputs
        self.df_ia_agg = None
        self.df_ia_agg_std = None

    def agg_by_ia(self, df):
        '''
        Aggregate the count of the TRD_TRADE_ID column for this month, and 12 month median value for each IA.
        TRD_TRADE_ID is not always the trade id in current usage
        '''
        cols_to_keep = ['IA_NAME', 'TRD_TRADE_ID']
        trd_agg = df.groupby(['TRD_MONTH', 'IA_NAME'])['TRD_TRADE_ID'].count().reset_index()

        trd_agg_tm = trd_agg[trd_agg['TRD_MONTH'] == 'JAN-17'].groupby('IA_NAME')['TRD_TRADE_ID'].sum().reset_index()[
            cols_to_keep]
        trd_agg_12m = (trd_agg.groupby('IA_NAME')['TRD_TRADE_ID'].median()).reset_index()[cols_to_keep]

        return trd_agg_tm, trd_agg_12m

    def complaints_by_ia(self, df):
        '''
        Aggregate sum of complaints for this month and all time
        '''
        cols_to_keep = ['IA_NAME', 'TRD_TRADE_ID']
        trd_agg = df.groupby(['TRD_MONTH', 'IA_NAME'])['TRD_TRADE_ID'].count().reset_index()

        trd_agg_tm = trd_agg[trd_agg['TRD_MONTH'] == 'JAN-17'].groupby('IA_NAME')['TRD_TRADE_ID'].sum().reset_index()[
            cols_to_keep]
        trd_agg_ever = (trd_agg.groupby('IA_NAME')['TRD_TRADE_ID']).sum().reset_index()[cols_to_keep]

        return trd_agg_tm, trd_agg_ever

    def agg_by_ia_sum(self, df, sum_col):
        '''
        Same agg agg_by_ia but using sum instead of count
        '''
        cols_to_keep = ['IA_NAME', sum_col]
        trd_agg = df.groupby(['TRD_MONTH', 'IA_NAME'])[sum_col].sum().reset_index()

        trd_agg_tm = trd_agg[trd_agg['TRD_MONTH'] == 'JAN-17'].groupby('IA_NAME')[sum_col].sum().reset_index()[cols_to_keep]
        trd_agg_12m = (trd_agg.groupby('IA_NAME')[sum_col].median()).reset_index()[cols_to_keep]

        return trd_agg_tm, trd_agg_12m

    def merge_by_ia(self, df_left, df_right, new_col_name='AggValue', col_to_retrieve='TRD_TRADE_ID'):
        '''
        left join an IA aggregate dataframe(left) with another dataframe (right) and rename new column
        '''
        df_ia_agg = pd.merge(left=df_left, right=df_right, on='IA_NAME', how='left')
        df_ia_agg[new_col_name] = df_ia_agg[col_to_retrieve]
        df_ia_agg[new_col_name] = df_ia_agg[new_col_name].fillna(0)
        del df_ia_agg[col_to_retrieve]

        return df_ia_agg

    def agg_by_other(self, trades, col_to_agg_by):
        '''
        Aggregate using another columns (such as region or branch)
        '''
        cols_to_keep = ['IA_NAME', 'TRD_TRADE_ID']
        trd_agg1 = trades.groupby(['TRD_MONTH', col_to_agg_by, 'IA_NAME'])['TRD_TRADE_ID'].count().reset_index()
        trd_agg = trd_agg1.groupby(['TRD_MONTH', col_to_agg_by])['TRD_TRADE_ID'].median().reset_index()

        trd_agg_tm = trd_agg[trd_agg['TRD_MONTH'] == 'JAN-17']
        trd_agg_tm = pd.merge(trd_agg_tm, self.ia_branch_region_bridge, on=col_to_agg_by, how='inner')[cols_to_keep]
        trd_agg_12m = trd_agg1.groupby(col_to_agg_by)['TRD_TRADE_ID'].median().reset_index()
        trd_agg_12m = pd.merge(trd_agg_12m, self.ia_branch_region_bridge, on=col_to_agg_by, how='inner')[cols_to_keep]
        return trd_agg_tm, trd_agg_12m

    def agg_by_other_sum(self, trades, col_to_agg_by, sum_col):
        '''
        Same as agg_by_other but using sum instead of count
        '''
        cols_to_keep = ['IA_NAME', sum_col]
        trd_agg1 = trades.groupby(['TRD_MONTH', col_to_agg_by, 'IA_NAME'])[sum_col].sum().reset_index()
        trd_agg = trd_agg1.groupby(['TRD_MONTH', col_to_agg_by])[sum_col].median().reset_index()

        trd_agg_tm = trd_agg[trd_agg['TRD_MONTH'] == 'JAN-17']
        trd_agg_tm = pd.merge(trd_agg_tm, self.ia_branch_region_bridge, on=col_to_agg_by, how='inner')[cols_to_keep]
        trd_agg_12m = trd_agg1.groupby(col_to_agg_by)[sum_col].median().reset_index()
        trd_agg_12m = pd.merge(trd_agg_12m, self.ia_branch_region_bridge, on=col_to_agg_by, how='inner')[cols_to_keep]
        return trd_agg_tm, trd_agg_12m

    def create_merged_cols(self, df_ia_agg, df_to_agg, new_col_name='AggValue', sum_col=None):
        '''
        Create columns for this month, twelve month median, and the same for branch and region values
        Note: branch / region currently commented out to exclude this functionality since it was adding too many features
        '''
        suffix_tm = '_TM'
        suffix_12m = '_12M'
        col_tm = new_col_name + suffix_tm
        col_12m = new_col_name + suffix_12m
        col_branch_tm = col_tm + '_BRANCH'
        col_branch_12m = col_12m + '_BRANCH'
        col_region_tm = col_tm + '_REGION'
        col_region_12m = col_12m + '_REGION'

        if 'Flagged' in new_col_name or new_col_name == 'Commission':

            agg_tm, agg_12m = self.agg_by_ia_sum(df_to_agg, sum_col)
            branch_agg_tm, branch_agg_12m = self.agg_by_other_sum(df_to_agg, col_to_agg_by='WM_PHYSICAL_BRANCH_ID',
                sum_col=sum_col)
            region_agg_tm, region_agg_12m = self.agg_by_other_sum(df_to_agg, col_to_agg_by='WM_PHY_BRANCH_REGION',
                sum_col=sum_col)
            col_to_retrieve = sum_col
        else:
            if new_col_name == 'Complaints':
                agg_tm, agg_12m = self.complaints_by_ia(df_to_agg)
            else:
                agg_tm, agg_12m = self.agg_by_ia(df_to_agg)
            branch_agg_tm, branch_agg_12m = self.agg_by_other(df_to_agg, col_to_agg_by='WM_PHYSICAL_BRANCH_ID')
            region_agg_tm, region_agg_12m = self.agg_by_other(df_to_agg, col_to_agg_by='WM_PHY_BRANCH_REGION')
            col_to_retrieve = 'TRD_TRADE_ID'
        df_agg_cur = self.merge_by_ia(df_ia_agg, agg_tm, col_tm, col_to_retrieve)
        df_agg_cur = self.merge_by_ia(df_agg_cur, agg_12m, col_12m, col_to_retrieve)

        '''
        # region and branch related - excluded for now
        df_agg_cur = merge_by_ia(df_agg_cur,branch_agg_tm,col_branch_tm,col_to_retrieve)
        df_agg_cur = merge_by_dia(df_agg_cur,branch_agg_12m,col_branch_12m,col_to_retrieve)

        df_agg_cur = merge_by_ia(df_agg_cur,region_agg_tm,col_region_tm,col_to_retrieve)
        df_agg_cur = merge_by_ia(df_agg_cur,region_agg_12m,col_region_12m,col_to_retrieve)

        if not new_col_name.lower() == 'complaints':
            df_agg_cur[new_col_name + '_TM_VS_12M'] = df_agg_cur[col_tm] - df_agg_cur[col_12m]
            df_agg_cur[new_col_name + '_TM_VS_MEDIAN'] = df_agg_cur[col_tm] - df_agg_cur[col_tm].median()
            df_agg_cur[new_col_name + '_12M_VS_MEDIAN'] = df_agg_cur[col_12m] - df_agg_cur[col_12m].median()
​
            df_agg_cur[new_col_name + '_TM_VS_BRANCH_TM'] = df_agg_cur[col_tm] - df_agg_cur[col_branch_tm]
            df_agg_cur[new_col_name + '_12M_VS_BRANCH_12M'] = df_agg_cur[col_12m] - df_agg_cur[col_branch_12m]
            df_agg_cur[new_col_name + '_TM_VS_REGION_TM'] = df_agg_cur[col_tm] - df_agg_cur[col_region_tm]
            df_agg_cur[new_col_name + '_12M_VS_REGION_12M'] = df_agg_cur[col_12m] - df_agg_cur[col_region_12m]

        del df_agg_cur[col_branch_tm]
        del df_agg_cur[col_branch_12m]
        del df_agg_cur[col_region_tm]
        del df_agg_cur[col_region_12m]
        '''

        if new_col_name.lower() == 'complaints':
            df_agg_cur.columns = [el.replace('12M', 'AllTime') for el in list(df_agg_cur.columns)]

        return df_agg_cur

    def run_step(self):
        
        self.acct_trades = pickle.load(open(self.pickled_dir + '/acct_trades.2.pkl', 'rb'))
        self.ia_branch_region_bridge = pickle.load(open(self.pickled_dir + '/ia_branch_region_bridge.pkl', 'rb'))
        self.flagged_extended = pickle.load(open(self.pickled_dir + '/flagged_extended.pkl', 'rb'))
        self.dummy_cols = pickle.load(open(self.pickled_dir + '/dummy_cols.pkl', 'rb'))
        self.code_dict = pickle.load(open(self.pickled_dir + '/code_dict.pkl', 'rb'))
        self.pro_trades = pickle.load(open(self.pickled_dir + '/pro_trades.pkl', 'rb'))
        self.cancelled_trades = pickle.load(open(self.pickled_dir + '/cancelled_trades.pkl', 'rb'))
        self.complaints = pickle.load(open(self.pickled_dir + '/complaints.2.pkl', 'rb'))
        self.traded_under_other_ia = pickle.load(open(self.pickled_dir + '/traded_under_other_ia.pkl', 'rb'))
        self.acct_kyc_12m = pickle.load(open(self.pickled_dir + '/acct_kyc_12m.pkl', 'rb'))

        ## Create the aggregated dataframe by IA

        # create dataframe with all unique Account level IAs to start
        self.df_ia_agg = pd.DataFrame()
        self.df_ia_agg['IA_NAME'] = self.acct_trades['IA_NAME'].unique()

        # add number of trades
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.acct_trades, new_col_name='Trades')

        # add each flag type
        #for col in self.dummy_cols:
         #   self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.flagged_extended, 
          #      new_col_name=f'Flagged_{self.code_dict[col]}', sum_col=col)

        # add total flagged trades (excluded for now since each flag type is included separately)
        # self.df_ia_agg = create_merged_cols(self.df_ia_agg,flagged_trades,'Flagged_Trades')

        # add number of pro trades
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.pro_trades, new_col_name='Pro_Trades')

        # add number of cancelled trades
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.cancelled_trades, new_col_name='Cancelled_Trades')

        # add number of complaints
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.complaints, new_col_name='Complaints')

        # add commission amounts
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.acct_trades, 
            new_col_name='Commission', sum_col='TRD_COMMISSION')

        # add trades under different IA
        self.df_ia_agg = self.create_merged_cols(self.df_ia_agg, self.traded_under_other_ia, 
            new_col_name='Trades_Under_Different_IA')

        # add number of clients with more than 2 KYC changes
        self.df_ia_agg = self.merge_by_ia(self.df_ia_agg, self.acct_kyc_12m, 
            new_col_name='Clients_With_More_Than_One_KYC_Change', col_to_retrieve='KYC_Hash')

        # cleanup and standardization (convert values to mean 0, std 1)
        self.df_ia_agg.index = self.df_ia_agg['IA_NAME']
        del self.df_ia_agg['IA_NAME']
        self.df_ia_agg_std = (self.df_ia_agg - self.df_ia_agg.mean(axis=0)) / self.df_ia_agg.std(axis=0)

        # replace any missing values with 0
        self.df_ia_agg_std = self.df_ia_agg_std.fillna(0)

        # intermediate output of dataframe
        #df_ia_agg_std.to_excel(f'IA_Aggregations_{this_run_id}.xls')
        self.df_ia_agg_std.to_excel(self.pickled_dir + '/IA_Aggregations_02.xls')
        
        pickle.dump(self.df_ia_agg, open(self.pickled_dir + '/df_ia_agg.pkl', 'wb'))
        pickle.dump(self.df_ia_agg_std, open(self.pickled_dir + '/df_ia_agg_std.pkl', 'wb'))
    