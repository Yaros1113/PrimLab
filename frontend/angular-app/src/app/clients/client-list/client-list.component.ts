import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ClientsService } from '../../services/clients.service';
import { Client } from '../../models/client.model';

@Component({
  selector: 'app-client-list',
  templateUrl: './client-list.component.html',
  styleUrls: ['./client-list.component.css']
})
export class ClientListComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'email', 'registrationDate', 'phoneNumbers', 'actions'];
  dataSource: MatTableDataSource<Client> = new MatTableDataSource<Client>([]);
  
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  search = '';
  sortField = 'Name';
  ascending = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private clientsService: ClientsService,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.loadClients();
  }

  loadClients() {
    this.clientsService.getClients(
      this.pageIndex + 1,
      this.pageSize,
      this.search,
      this.sortField,
      this.ascending
    ).subscribe({
      next: (data) => {
        this.dataSource.data = data.items;
        this.totalItems = data.totalCount;
      },
      error: (error) => console.error('Error loading clients:', error)
    });
  }

  onPageChange(event: any) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadClients();
  }

  onSortChange(event: any) {
    this.sortField = event.active;
    this.ascending = event.direction === 'asc';
    this.loadClients();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.loadClients();
  }

  editClient(id: number) {
    this.router.navigate(['/clients', id, 'edit']);
  }

  createClient() {
    this.router.navigate(['/clients', 'new']);
  }
}