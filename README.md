# TrafficData
Seattle Traffic Data Project

## Purpose
The purpose of this project is to practice a data science project using Seattle traffic data.

Some of the longterm goals of the project:
- explore data science/software tools (AzureML, Jupyter Notebooks, Azure Function Apps, etc)
- be able to predict the evening commute hours in advance
- look for trends over time on the different routes in the region

## Data
- WSDOT API: http://wsdot.com/traffic/api/ 

## Components

### DataArchiver
`/src/dotnot/DataArchiver`
This is an Azure Function App dedicated to retrieving the current travel times from the WSDOT Travel Times API every 20 minutes.
It stores the data in a SQL table, also in Azure.

### DataViewer



## Who
- Trevor Blanarik