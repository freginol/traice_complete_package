from traice import load
from traice import merge
from traice import breakout
from traice import aggregate
from traice import fit
from traice import hitlist
from traice import hitlistexp
from traice import kridetails
from traice import wellbeing
from traice import branchbin

INPUT_DIR = '/Users/fabianreginold/Downloads/traice/TrAIce/csvfiles'
PICKLED_DIR = '/Users/fabianreginold/Downloads/traice/TrAIce/csvfiles/pickled/batch'
COMPLETION_DIR = '/Users/fabianreginold/Downloads/traice/TrAIce/csvfiles/traice_completion'

# Load all of the LEX*.csv files
input_files = [
    INPUT_DIR + '/LEX_TRADE.csv',
    INPUT_DIR + '/LEX_ACCT_TRADE_BRIDGE.csv',
    INPUT_DIR + '/LEX_ACCT.csv',
    INPUT_DIR + '/LEX_TRD_IND.csv',
    INPUT_DIR + '/LEX_COMPLAINTS.csv',
    INPUT_DIR + '/LEX_GROSS_IA_INCOME.csv'
]
load.Load(input_files, PICKLED_DIR, COMPLETION_DIR + '/load.complete').run()

# Merge the tables using joins to isolate the most important data
input_files = [
    PICKLED_DIR + '/acct.pkl',
    PICKLED_DIR + '/trades.pkl',
    PICKLED_DIR + '/trd_ind.pkl',
    PICKLED_DIR + '/acct_trade_bridge.pkl',
    INPUT_DIR + '/trade indicator.xlsx'
]
merge.Merge(input_files, PICKLED_DIR, COMPLETION_DIR + '/merge.complete').run()

# Break out each of the following indicators:
# - Pro trades: IA traded on their own account
# - Cancelled: Cancelled trades
# - Reversals: Taking a buy position and sell position that together net out to 0, 
#   indicating needless trading
# - Complaints: customer complaints
# - KYC Changes: Know Your Customer changes in the last 12 months
# - Trading under different IA: IA traded under another IA's name (not always an
#   indicator of questionable behavior) 
input_files = [
    PICKLED_DIR + '/acct_trades.pkl',
    PICKLED_DIR + '/complaints.pkl'
]
breakout.BreakOut(input_files, PICKLED_DIR, COMPLETION_DIR + '/breakout.complete').run()

# Aggregate by IA
input_files = [
    PICKLED_DIR + '/acct_trades.2.pkl',
    PICKLED_DIR + '/flagged_extended.pkl',
    PICKLED_DIR + '/code_dict.pkl',
    PICKLED_DIR + '/pro_trades.pkl',
    PICKLED_DIR + '/cancelled_trades.pkl',
    PICKLED_DIR + '/complaints.2.pkl',
    PICKLED_DIR + '/traded_under_other_ia.pkl',
    PICKLED_DIR + '/acct_kyc_12m.pkl'    
]
aggregate.Aggregate(input_files, PICKLED_DIR, COMPLETION_DIR + '/aggregate.complete').run()

# Fit our anomaly detection model
input_files = [
    PICKLED_DIR + '/df_ia_agg.pkl',
    PICKLED_DIR + '/df_ia_agg_std.pkl'
]
fit.Fit(input_files, PICKLED_DIR, COMPLETION_DIR + '/fit.complete').run()

# Generate hit list 
input_files = [
    PICKLED_DIR + '/df_ia_agg_scored.pkl',
]
hitlist.HitList(input_files, PICKLED_DIR, COMPLETION_DIR + '/hitlist.complete').run()

input_files = [
    PICKLED_DIR + '/hit_list.pkl',
    PICKLED_DIR + '/gross_revs.pkl',
    PICKLED_DIR + '/acct_trades.2.pkl'
]
hitlistexp.HitListExp(input_files, PICKLED_DIR, COMPLETION_DIR + '/hitlistexp.complete').run()

input_files = [
    INPUT_DIR + '/GUI KRI Naming.xlsx',
    PICKLED_DIR + '/df_ia_agg.pkl',
    PICKLED_DIR + '/df_ia_agg_scored.pkl',
    PICKLED_DIR + '/hit_list_expanded.pkl'
]
kridetails.KRIDetails(input_files, PICKLED_DIR, COMPLETION_DIR + '/kridetails.complete').run()

input_files = [
    PICKLED_DIR + '/hit_list.pkl'
]
wellbeing.Wellbeing(input_files, PICKLED_DIR, COMPLETION_DIR + '/wellbeing.complete').run()

"""
        self.hit_list_expanded = pickle.load(open(self.pickled_dir + '/hit_list_expanded.pkl', 'rb'))
        self.ia_kri_mapping = pickle.load(open(self.pickled_dir + '/ia_kri_mapping.pkl', 'rb'))
"""
input_files = [
    PICKLED_DIR + '/hit_list_expanded.pkl',
    PICKLED_DIR + '/ia_kri_mapping.pkl'
]
branchbin.BranchBin(input_files, PICKLED_DIR, COMPLETION_DIR + '/branchbin.complete').run()
