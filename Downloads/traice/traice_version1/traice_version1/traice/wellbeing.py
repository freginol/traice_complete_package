import pickle
import numpy as np
import pandas as pd
from pathlib import Path
#from plotnine import *

from traice import batchstep

class Wellbeing(batchstep.BatchStep):

    def __init__(self, input_dir, pickled_dir, out_path):

        super().__init__(input_dir, pickled_dir, out_path)

        # Inputs
        self.hit_list = None

        # Outputs
        self.wellbeing_df = None

    def run_step(self):
            
        # TODO check this
        np.random.seed(123)

        self.hit_list = pickle.load(open(self.pickled_dir + '/hit_list.pkl', 'rb'))

        # create template columns for IA behaviour
        ia_repeated = np.repeat(self.hit_list['IA ID'].values,12)
        months_repeated = np.tile([5,5,5,6,6,6,7,7,7,8,8,8],len(self.hit_list['IA ID'].values))
        desc_repeated = np.tile(['anxiety','fulfillment','judgement'],int(len(ia_repeated)/3))

        # generate anxiety, fulfilment, and judgement scores
        a = []
        for i in range(len(self.hit_list['IA ID']) * 4):
            if i % 4 == 0:
                # start new anxiety score after four months
                a.append(np.random.randint(10, 80, 1)[0])
            else:
                # create a new anxiety score and random amount above or below the previous score
                a.append(int(min(80, max(10, a[i - 1] + [-1, 1][np.random.choice(2)] * int(3 * np.random.random(1))))))

        # create anxiety, fulfilment, and judgement numpy arrays
        a = np.array(a)
        f = 100 - a + 20
        j = 100 - a + 10

        # correct instances where above 100 to be equal to 100
        new_arr = []
        for i in range(len(a)):

            for k in range(3):

                if k == 0:
                    new_arr.append(a[i])
                elif k == 1:
                    if f[i] > 100:
                        new_arr.append(100)
                    else:
                        new_arr.append(f[i])
                else:
                    if j[i] > 100:
                        new_arr.append(100)
                    else:
                        new_arr.append(j[i])

        # test plotting of scores for small section of values
        df_test = pd.DataFrame({'a':a,'j':j,'f':f,'i':[i for i in range(len(a))]})
        # TODO get plotnine working
        #(ggplot(data=df_test) + geom_line(aes('i','a')) + geom_line(aes('i','f')) + geom_line(aes('i','j')) + scale_x_continuous(limits=[8,16]))

        # output wellbeing values to excel
        self.wellbeing = pd.DataFrame({'IA ID':ia_repeated,
            'Month': months_repeated,
            'Metric description': desc_repeated,
            'Value':new_arr})
        #self.wellbeing.to_excel(f'wellbeing_{this_run_id}.xlsx',index=False)
        self.wellbeing.to_excel(self.pickled_dir + '/wellbeing_02.xlsx',index=False)

        pickle.dump(self.wellbeing, open(self.pickled_dir + '/wellbeing.pkl', 'wb'))
