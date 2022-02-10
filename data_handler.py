from cgi import test
import os, glob, pandas
from re import T
import statistics

cwd = os.getcwd()
data_path = f"{cwd}/Visual Search Experiment Project/Assets/Data/"

# Collect paths of raw data csv's
def get_raw_data(condition_directory):
    return [file.replace("\\", "/") for file in glob.glob(f"{data_path}{condition_directory}/*.csv")]

lab_path = get_raw_data(2)
desert_path = get_raw_data(3)
space_path = get_raw_data(4)

# Create a data frame for each condition 
headers = ['condition', 'PID', '4-Target', '4-NoTarget', '12-Target', '12-NoTarget', 'trialsFailed']

lab_room_data = pandas.DataFrame(columns=headers)
desert_data = pandas.DataFrame(columns=headers)
space_data = pandas.DataFrame(columns=headers)

def handle_data(raw_data_array, target_dataframe):
    for csv_path in raw_data_array:
        datafile = pandas.read_csv(csv_path)
        data_to_export = {}
        target_4, no_target_4, target_12, no_target_12 = ([], [], [], []) 

        data_to_export['condition'] = datafile['condition'].iloc[0]
        data_to_export['PID'] = datafile['PID'].iloc[0]
        trials_failed = 0

        for index, row in datafile.iterrows():
            if row['trialPassed'] == True:
                array_size = row['arraySize'] 
                target_shown = row['targetShown']
                reaction_time = row['reactionTime']

                if array_size == 4:
                    if target_shown == True:
                        target_4.append(reaction_time)
                    else:
                        no_target_4.append(reaction_time)
                elif array_size == 12:
                    if target_shown == True:
                        target_12.append(reaction_time)
                    else:
                        no_target_12.append(reaction_time)
            else:
                trials_failed += 1
                
        data_to_export['trialsFailed'] = trials_failed
        data_to_export['4-Target'] = statistics.mean(target_4)
        data_to_export['4-NoTarget'] = statistics.mean(no_target_4)
        data_to_export['12-Target'] = statistics.mean(target_12)
        data_to_export['12-NoTarget'] = statistics.mean(no_target_12)

        df_dictionary = pandas.DataFrame([data_to_export])
        target_dataframe = pandas.concat([target_dataframe, df_dictionary], ignore_index=True)

    return(target_dataframe)


lab_room_data = handle_data(lab_path, lab_room_data)
desert_data = handle_data(desert_path, desert_data)
space_data = handle_data(space_path, space_data)


# Add headers to each dataframe

# Add necessary data 

# Export as seperate pages of 1 excel doc
writer = pandas.ExcelWriter(f'{data_path}env-pilot-datafile.xlsx')
lab_room_data.to_excel(writer, 'lab', index=False, header=True)
desert_data.to_excel(writer, 'desert', index=False, header=True)
space_data.to_excel(writer, 'space', index=False, header=True)
writer.save()