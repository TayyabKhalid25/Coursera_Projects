from django.urls import path
from . import views

urlpatterns = [
    # Menu items endpoints
    path('menu-items', views.MenuItemListCreateView.as_view(), name='menu-items-list'),
    path('menu-items/<int:pk>', views.MenuItemDetailView.as_view(), name='menu-items-detail'),
    
    # User group management endpoints
    path('groups/manager/users', views.ManagerListView.as_view(), name='manager-list'),
    path('groups/manager/users/<int:pk>', views.ManagerDetailView.as_view(), name='manager-detail'),
    path('groups/delivery-crew/users', views.DeliveryCrewListView.as_view(), name='delivery-crew-list'),
    path('groups/delivery-crew/users/<int:pk>', views.DeliveryCrewDetailView.as_view(), name='delivery-crew-detail'),
    
    # Cart endpoints
    path('cart/menu-items', views.CartListCreateDeleteView.as_view(), name='cart-list'),
    
    # Order endpoints
    path('orders', views.OrderListCreateView.as_view(), name='orders-list'),
    path('orders/<int:pk>', views.OrderDetailView.as_view(), name='orders-detail'),
    
    # Ratings endpoint
    path('ratings', views.RatingsView.as_view(), name='ratings-list'),
]
