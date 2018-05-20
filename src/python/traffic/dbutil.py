import pyodbc 
import configparser
import pandas

config = configparser.ConfigParser()
config.read('../../../_private/db.config')

connection_string = ('Driver={{ODBC Driver 13 for SQL Server}};'
                     'Server=tcp:{server};'
                     'Database={database};'
                     'Uid={uid};'
                     'Pwd={dbpassword};'
                     'Encrypt=yes;'
                     'TrustServerCertificate=no;'
                     'Connection Timeout=30;').format(**config['CONNECTION'])

def get_rows(tid):
    tid = int(tid)
    conn = pyodbc.connect(connection_string)
    query = "SELECT TimeUpdated,CurrentTime,Description FROM TravelTimes WHERE TRAVELTIMEID = {0}".format(tid)
    df =  pandas.read_sql_query(query, conn)
    return df.sort_values(by=['TimeUpdated'])