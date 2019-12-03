from django.http import HttpResponse
from django.template import loader
from django.views.decorators.csrf import csrf_exempt
import numpy as np
from . import simplex


def index(request):
    template = loader.get_template('simplex/index.html')
    context = {
        'X': range(1,5),
        'Y': range(1,5)
    }
    return HttpResponse(template.render(context, request))

@csrf_exempt

def calc(request):
    x_size = 4
    y_size = 4
    array = [[0 for x in range(1,y_size + 1)] for y in range(1,x_size + 1)] 
    if request.method == 'POST':
        for i in range(1,x_size + 1):
            for j in range(1,y_size + 1):
                array[i-1][j-1] = int(request.POST.get("X{}Y{}".format(i,j)))
        numpyArray = np.array(array)
        result = simplex.simplex(numpyArray)
        resultMatrix = result.get('matrix')
        resultValue = result.get('values')
        template = loader.get_template('simplex/result.html')
        context = {
            'resMat': resultMatrix,
            'resVal': resultValue
        }
        return HttpResponse(template.render(context, request))


