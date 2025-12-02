from django.test import TestCase
from rest_framework.test import APIClient
from rest_framework import status
from restaurant.models import Menu
from restaurant.serializers import MenuSerializer

class MenuViewTest(TestCase):
    def setUp(self):
        self.client = APIClient()
        self.menu1 = Menu.objects.create(ID=1, Title="IceCream", Price=80, Inventory=100)
        self.menu2 = Menu.objects.create(ID=2, Title="Burger", Price=120, Inventory=50)
        self.menu3 = Menu.objects.create(ID=3, Title="Pizza", Price=200, Inventory=20)

    def test_getall(self):
        # Retrieve all Menu objects
        response = self.client.get('/restaurant/menu/')
        
        # Get all menu items from database
        menus = Menu.objects.all()
        serializer = MenuSerializer(menus, many=True)
        
        # Check if the request was successful
        self.assertEqual(response.status_code, status.HTTP_200_OK)
        
        # Check if the serialized data equals the response
        # We compare the data. Note: response.data might be OrderedDicts, serializer.data is also a list of dicts/OrderedDicts
        self.assertEqual(response.data, serializer.data)
