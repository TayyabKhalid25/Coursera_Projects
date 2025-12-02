from django.db import models


# Create your models here.
class Booking(models.Model):
    ID = models.IntegerField(primary_key=True, auto_created=True, default=0) 
    Name = models.CharField(max_length=255)
    No_of_guests = models.IntegerField(null = False)
    BookingDate = models.DateField(null = False)
    
    def __str__(self): 
        return self.Name


# Add code to create Menu model
class Menu(models.Model):
    ID = models.IntegerField(primary_key=True, auto_created=True, default=0) 
    Title = models.CharField(max_length=255) 
    Price = models.DecimalField(max_digits=10, decimal_places=2)
    Inventory = models.IntegerField(null = False)
    
    def __str__(self):
        return f'{self.Title} : {str(self.Price)}'
    