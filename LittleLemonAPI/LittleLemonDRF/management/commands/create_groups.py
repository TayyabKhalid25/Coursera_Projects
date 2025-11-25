from django.core.management.base import BaseCommand
from django.contrib.auth.models import Group

class Command(BaseCommand):
    help = 'Create Manager and Delivery crew groups'

    def handle(self, *args, **options):
        manager_group, created = Group.objects.get_or_create(name='Manager')
        delivery_crew_group, created = Group.objects.get_or_create(name='Delivery crew')
        
        self.stdout.write(self.style.SUCCESS('Groups created successfully'))
