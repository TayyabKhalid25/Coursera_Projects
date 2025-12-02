from django.urls import path, include
from . import views
from rest_framework.authtoken.views import obtain_auth_token
from rest_framework.routers import DefaultRouter

router = DefaultRouter()
router.register(r'booking', views.BookingViewSet)

urlpatterns = [
    path('', views.index, name='index'),
    path('', include(router.urls)),
    path('api-token-auth/', obtain_auth_token),
    path('menu/', views.MenuItemsView.as_view(), name="menu"),
    path('menu_item/<int:pk>/', views.SingleMenuItemView.as_view(), name="menu_item"),  
]
