import pyodbc 
import configparser

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
    cursor = conn.cursor()
    rows = cursor.execute("SELECT TimeUpdated,CurrentTime,Description FROM TravelTimes WHERE TRAVELTIMEID = {0}".format(tid)).fetchall()
    return sorted(rows, key=lambda x: x[0])