from django.http import HttpResponse
from django.template import loader
from django.views.decorators.csrf import csrf_exempt


def index(request):
    template = loader.get_template('linear/index.html')
    context = {
    }
    return HttpResponse(template.render(context, request))

@csrf_exempt
def calc(request):
    if request.method == 'POST':
        first = request.POST.get("first")
        second = request.POST.get("second")
        total = int(first) + int(second)
        template = loader.get_template('add/result.html')
        context = {
            'first': first,
            'second': second,
            'total': total
        }
        return HttpResponse(template.render(context, request))


