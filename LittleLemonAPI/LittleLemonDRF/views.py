from rest_framework import generics, status, viewsets
from rest_framework.decorators import api_view
from rest_framework.response import Response
from rest_framework.permissions import IsAuthenticated, AllowAny
from rest_framework.throttling import UserRateThrottle, AnonRateThrottle
from django.shortcuts import get_object_or_404
from django.contrib.auth.models import User, Group
from decimal import Decimal
from .models import MenuItem, Cart, Order, OrderItem, Rating
from .serializers import (
    MenuItemSerializer, CartSerializer, OrderSerializer, 
    OrderItemSerializer, RatingSerializer, UserSerializer
)

# Permissions
class IsManager(generics.GenericAPIView):
    def check_manager_permission(self):
        return self.request.user.groups.filter(name='Manager').exists()

class IsDeliveryCrew(generics.GenericAPIView):
    def check_delivery_crew_permission(self):
        return self.request.user.groups.filter(name='Delivery crew').exists()

# Menu Items Views
class MenuItemListCreateView(generics.ListCreateAPIView):
    queryset = MenuItem.objects.all()
    serializer_class = MenuItemSerializer
    throttle_classes = [AnonRateThrottle, UserRateThrottle]
    search_fields = ['title', 'category']
    ordering_fields = ['title', 'price']
    
    def get_permissions(self):
        if self.request.method == 'GET':
            return [AllowAny()]
        return [IsAuthenticated()]
    
    def create(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access. Only managers can create menu items."},
                status=status.HTTP_403_FORBIDDEN
            )
        return super().create(request, *args, **kwargs)

class MenuItemDetailView(generics.RetrieveUpdateDestroyAPIView):
    queryset = MenuItem.objects.all()
    serializer_class = MenuItemSerializer
    throttle_classes = [AnonRateThrottle, UserRateThrottle]
    lookup_field = 'pk'
    
    def get_permissions(self):
        if self.request.method == 'GET':
            return [AllowAny()]
        return [IsAuthenticated()]
    
    def get_object(self):
        try:
            return super().get_object()
        except:
            return None
    
    def retrieve(self, request, *args, **kwargs):
        instance = self.get_object()
        if instance is None:
            return Response(
                {"detail": "Menu item not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        serializer = self.get_serializer(instance)
        return Response(serializer.data)
    
    def update(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access. Only managers can update menu items."},
                status=status.HTTP_403_FORBIDDEN
            )
        instance = self.get_object()
        if instance is None:
            return Response(
                {"detail": "Menu item not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        return super().update(request, *args, **kwargs)
    
    def destroy(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access. Only managers can delete menu items."},
                status=status.HTTP_403_FORBIDDEN
            )
        instance = self.get_object()
        if instance is None:
            return Response(
                {"detail": "Menu item not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        return super().destroy(request, *args, **kwargs)

# User Group Management Views
class ManagerListView(generics.ListCreateAPIView):
    serializer_class = UserSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def get_queryset(self):
        return User.objects.filter(groups__name='Manager')
    
    def list(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        return super().list(request, *args, **kwargs)
    
    def create(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        username = request.data.get('username')
        if not username:
            return Response(
                {"detail": "Username is required."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        try:
            user = User.objects.get(username=username)
        except User.DoesNotExist:
            return Response(
                {"detail": "User not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        manager_group = Group.objects.get(name='Manager')
        user.groups.add(manager_group)
        return Response(
            {"detail": "User added to Manager group."},
            status=status.HTTP_201_CREATED
        )

class ManagerDetailView(generics.DestroyAPIView):
    serializer_class = UserSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def destroy(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        user_id = kwargs.get('pk')
        try:
            user = User.objects.get(pk=user_id)
        except User.DoesNotExist:
            return Response(
                {"detail": "User not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        manager_group = Group.objects.get(name='Manager')
        user.groups.remove(manager_group)
        return Response(status=status.HTTP_200_OK)

class DeliveryCrewListView(generics.ListCreateAPIView):
    serializer_class = UserSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def get_queryset(self):
        return User.objects.filter(groups__name='Delivery crew')
    
    def list(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        return super().list(request, *args, **kwargs)
    
    def create(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        username = request.data.get('username')
        if not username:
            return Response(
                {"detail": "Username is required."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        try:
            user = User.objects.get(username=username)
        except User.DoesNotExist:
            return Response(
                {"detail": "User not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        delivery_crew_group = Group.objects.get(name='Delivery crew')
        user.groups.add(delivery_crew_group)
        return Response(
            {"detail": "User added to Delivery crew group."},
            status=status.HTTP_201_CREATED
        )

class DeliveryCrewDetailView(generics.DestroyAPIView):
    serializer_class = UserSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def destroy(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        user_id = kwargs.get('pk')
        try:
            user = User.objects.get(pk=user_id)
        except User.DoesNotExist:
            return Response(
                {"detail": "User not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        delivery_crew_group = Group.objects.get(name='Delivery crew')
        user.groups.remove(delivery_crew_group)
        return Response(status=status.HTTP_200_OK)

# Cart Views
class CartListCreateDeleteView(generics.ListCreateAPIView):
    serializer_class = CartSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def get_queryset(self):
        return Cart.objects.filter(user=self.request.user)
    
    def create(self, request, *args, **kwargs):
        menuitem_id = request.data.get('menuitem_id')
        quantity = request.data.get('quantity')
        
        if not menuitem_id or not quantity:
            return Response(
                {"detail": "Both menuitem_id and quantity are required."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        try:
            menuitem = MenuItem.objects.get(pk=menuitem_id)
        except MenuItem.DoesNotExist:
            return Response(
                {"detail": "Menu item not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        try:
            quantity = int(quantity)
            if quantity <= 0:
                raise ValueError
        except (ValueError, TypeError):
            return Response(
                {"detail": "Quantity must be a positive integer."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        unit_price = menuitem.price
        price = unit_price * quantity
        
        cart_item, created = Cart.objects.update_or_create(
            user=request.user,
            menuitem=menuitem,
            defaults={
                'quantity': quantity,
                'unit_price': unit_price,
                'price': price
            }
        )
        
        serializer = self.get_serializer(cart_item)
        return Response(serializer.data, status=status.HTTP_201_CREATED)
    
    def delete(self, request, *args, **kwargs):
        Cart.objects.filter(user=request.user).delete()
        return Response(status=status.HTTP_204_NO_CONTENT)

# Order Views
class OrderListCreateView(generics.ListCreateAPIView):
    serializer_class = OrderSerializer
    throttle_classes = [UserRateThrottle]
    search_fields = ['user__username']
    ordering_fields = ['date', 'total']
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def get_queryset(self):
        user = self.request.user
        if user.groups.filter(name='Manager').exists():
            return Order.objects.all()
        elif user.groups.filter(name='Delivery crew').exists():
            return Order.objects.filter(delivery_crew=user)
        else:
            return Order.objects.filter(user=user)
    
    def create(self, request, *args, **kwargs):
        # Get current user's cart items
        cart_items = Cart.objects.filter(user=request.user)
        
        if not cart_items.exists():
            return Response(
                {"detail": "Cart is empty. Add items to cart before placing an order."},
                status=status.HTTP_400_BAD_REQUEST
            )
        
        # Calculate total
        total = sum(item.price for item in cart_items)
        
        # Create order
        order = Order.objects.create(
            user=request.user,
            total=total,
            status=False
        )
        
        # Create order items from cart
        for cart_item in cart_items:
            OrderItem.objects.create(
                order=order,
                menuitem=cart_item.menuitem,
                quantity=cart_item.quantity,
                unit_price=cart_item.unit_price,
                price=cart_item.price
            )
        
        # Clear cart
        cart_items.delete()
        
        serializer = self.get_serializer(order)
        return Response(serializer.data, status=status.HTTP_201_CREATED)

class OrderDetailView(generics.RetrieveUpdateDestroyAPIView):
    serializer_class = OrderSerializer
    throttle_classes = [UserRateThrottle]
    
    def get_permissions(self):
        return [IsAuthenticated()]
    
    def get_queryset(self):
        user = self.request.user
        if user.groups.filter(name='Manager').exists():
            return Order.objects.all()
        elif user.groups.filter(name='Delivery crew').exists():
            return Order.objects.filter(delivery_crew=user)
        else:
            return Order.objects.filter(user=user)
    
    def retrieve(self, request, *args, **kwargs):
        try:
            instance = self.get_object()
        except Order.DoesNotExist:
            return Response(
                {"detail": "Order not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        serializer = self.get_serializer(instance)
        return Response(serializer.data)
    
    def update(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access. Only managers can update orders."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        try:
            instance = self.get_object()
        except Order.DoesNotExist:
            return Response(
                {"detail": "Order not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        return super().update(request, *args, **kwargs)
    
    def partial_update(self, request, *args, **kwargs):
        # Allow both Manager and Delivery crew to update status
        try:
            instance = self.get_object()
        except Order.DoesNotExist:
            return Response(
                {"detail": "Order not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        if request.user.groups.filter(name='Delivery crew').exists():
            # Delivery crew can only update status
            if 'status' in request.data and len(request.data) == 1:
                instance.status = request.data['status']
                instance.save()
                serializer = self.get_serializer(instance)
                return Response(serializer.data)
            else:
                return Response(
                    {"detail": "Delivery crew can only update the status field."},
                    status=status.HTTP_400_BAD_REQUEST
                )
        elif request.user.groups.filter(name='Manager').exists():
            return super().partial_update(request, *args, **kwargs)
        else:
            return Response(
                {"detail": "Unauthorized access."},
                status=status.HTTP_403_FORBIDDEN
            )
    
    def destroy(self, request, *args, **kwargs):
        if not request.user.groups.filter(name='Manager').exists():
            return Response(
                {"detail": "Unauthorized access. Only managers can delete orders."},
                status=status.HTTP_403_FORBIDDEN
            )
        
        try:
            instance = self.get_object()
        except Order.DoesNotExist:
            return Response(
                {"detail": "Order not found."},
                status=status.HTTP_404_NOT_FOUND
            )
        
        return super().destroy(request, *args, **kwargs)

# Ratings Views
class RatingsView(generics.ListCreateAPIView):
    queryset = Rating.objects.all()
    serializer_class = RatingSerializer
    throttle_classes = [AnonRateThrottle, UserRateThrottle]

    def get_permissions(self):
        if self.request.method == 'GET':
            return []
        return [IsAuthenticated()]
