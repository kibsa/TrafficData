import pandas

def filter_for_weekdays(df):
    return df[df['TimeUpdated'].dt.weekday < 5]

def filter_day_of_week(df, dow):
    return df[df['TimeUpdated'].dt.weekday == dow]

def filter_hour_range(df, lh, hh):
    return df[(df['TimeUpdated'].dt.hour >= lh) & (df['TimeUpdated'].dt.hour <= hh)]