import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ProductsService } from '../../services/products.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements AfterViewInit {
  displayedColumns: string[] = ['id', 'name', 'description', 'price', 'actions'];
  dataSource: MatTableDataSource<Product> = new MatTableDataSource<Product>([]);
  
  totalItems = 0;
  pageSize = 10;
  pageIndex = 0;
  search = '';
  sortField = 'Name';
  ascending = true;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor(
    private productsService: ProductsService,
    private router: Router
  ) {}

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
    this.loadProducts();
  }

  loadProducts() {
    this.productsService.getProducts(
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
      error: (error) => console.error('Error loading products:', error)
    });
  }

  onPageChange(event: any) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadProducts();
  }

  onSortChange(event: any) {
    this.sortField = event.active;
    this.ascending = event.direction === 'asc';
    this.loadProducts();
  }

  applyFilter() {
    this.pageIndex = 0;
    this.loadProducts();
  }

  editProduct(id: number) {
    this.router.navigate(['/products', id, 'edit']);
  }

  createProduct() {
    this.router.navigate(['/products', 'new']);
  }
}
