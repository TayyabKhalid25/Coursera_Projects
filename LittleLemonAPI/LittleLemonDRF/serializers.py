from rest_framework import serializers
from .models import MenuItem, Cart, Order, OrderItem, Rating
from rest_framework.validators import UniqueTogetherValidator
from django.contrib.auth.models import User

class UserSerializer(serializers.ModelSerializer):
    class Meta:
        model = User
        fields = ['id', 'username', 'email', 'first_name', 'last_name']

class MenuItemSerializer(serializers.ModelSerializer):
    class Meta:
        model = MenuItem
        fields = ['id', 'title', 'price', 'featured', 'category']

class CartSerializer(serializers.ModelSerializer):
    menuitem_id = serializers.IntegerField(write_only=True)
    menuitem = MenuItemSerializer(read_only=True)
    unit_price = serializers.DecimalField(max_digits=6, decimal_places=2, read_only=True)
    price = serializers.SerializerMethodField()
    
    class Meta:
        model = Cart
        fields = ['id', 'menuitem', 'menuitem_id', 'quantity', 'unit_price', 'price']
    
    def get_price(self, obj):
        return obj.price

class OrderItemSerializer(serializers.ModelSerializer):
    menuitem = MenuItemSerializer(read_only=True)
    
    class Meta:
        model = OrderItem
        fields = ['id', 'menuitem', 'quantity', 'unit_price', 'price']

class OrderSerializer(serializers.ModelSerializer):
    order_items = OrderItemSerializer(many=True, read_only=True)
    delivery_crew = UserSerializer(read_only=True)
    
    class Meta:
        model = Order
        fields = ['id', 'user', 'delivery_crew', 'status', 'total', 'date', 'order_items']
        read_only_fields = ['user', 'total', 'date']

class RatingSerializer(serializers.ModelSerializer):
    user = serializers.PrimaryKeyRelatedField(
            queryset=User.objects.all(),
            default=serializers.CurrentUserDefault()
    )

    class Meta:
        model = Rating
        fields = ['user', 'menuitem_id', 'rating']

        validators = [
            UniqueTogetherValidator(
                queryset=Rating.objects.all(),
                fields=['user', 'menuitem_id']
            )
        ]

        extra_kwargs = {
            'rating': {'min_value': 0, 'max_value': 5},
        }
