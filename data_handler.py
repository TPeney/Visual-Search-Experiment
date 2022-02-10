from cgi import test
import os, glob, pandas
from unittest import TestSuite

cwd = os.getcwd()

# Collect paths of raw data csv's

# Create a data frame for each condition 
headers = ['PID', '4-Target', '4-NoTarget', '12-Target', '4-NoTarget', 'RT', 'trialsFailed']
# Add if trial passed - else uptick fail counter

lab_room_data = pandas.DataFrame()
desert_data = pandas.DataFrame()
space_data = pandas.DataFrame()

"""
for each row in df
    add rt to appropriate condition array
calculate average rt
add data to temp df
add df to main data df
"""
 
# Add headers to each dataframe

# Add necessary data 

# Export as seperate pages of 1 excel doc