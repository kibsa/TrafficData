import matplotlib.pyplot as plt
from matplotlib.figure import Figure
from matplotlib.backends.backend_agg import FigureCanvasAgg as FigureCanvas
from matplotlib.dates import DateFormatter

def canvas_from_data(rows):
    name = rows[0][2].strip()
    fig=Figure()
    ax=fig.add_subplot(111)
    x=[r[0] for r in rows]
    y=[r[1] for r in rows]
    ax.plot_date(x, y, '-')
    ax.xaxis.set_major_formatter(DateFormatter('%Y-%m-%d %H:%M'))
    ax.set_title(name)
    fig.autofmt_xdate()
    canvas=FigureCanvas(fig)
    return canvas