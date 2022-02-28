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
data_directories = [lab_path, desert_path, space_path]

# Create a data frame for each condition 
headers = ['condition', 'PID', '4-Target', '4-NoTarget', '12-Target', '12-NoTarget', 'trialsFailed']


def append_data(csv_path, target_dataframe):
    datafile = pandas.read_csv(csv_path)

    datafile.replace(to_replace='3 - VR Full', value= 'Lab', inplace= True)  
    datafile["targetShown"].replace(to_replace='FALSE', value= 'Absent', inplace= True)  
    datafile["targetShown"].replace(to_replace='TRUE', value= 'Present', inplace= True)  

    datafile = datafile.round(0)

    datafile.drop('condition', axis= 1, inplace= True)
    datafile.drop('allVisualCues', axis= 1, inplace= True)
    datafile.drop('trialName', axis= 1, inplace= True)
    target_dataframe = pandas.concat([target_dataframe, datafile])
    return target_dataframe

def handle_data():
    raw_dataframe = pandas.DataFrame()
    current_index = 0
    while current_index < len(lab_path):
        raw_dataframe = append_data(lab_path[current_index], raw_dataframe)
        raw_dataframe = append_data(desert_path[current_index], raw_dataframe)
        raw_dataframe = append_data(space_path[current_index], raw_dataframe)
        current_index += 1
    return raw_dataframe

raw_dataframe = handle_data()

# Export as seperate pages of 1 excel doc
writer = pandas.ExcelWriter(f'{data_path}env-pilot-datafile-raw.xlsx')
raw_dataframe.to_excel(writer, 'raw_data', index=False, header=True)
writer.save()