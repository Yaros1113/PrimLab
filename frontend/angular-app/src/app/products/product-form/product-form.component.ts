import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductsService } from '../../services/products.service';
import { Product } from '../../models/product.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-product-form',
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.css']
})
export class ProductFormComponent implements OnInit {
  productForm: FormGroup;
  isEditMode = false;
  productId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private productsService: ProductsService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: ['', [Validators.required, Validators.min(0.01)]]
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.productId = +params['id'];
        this.loadProductData();
      }
    });
  }

  loadProductData() {
    if (this.productId) {
      this.loading = true;
      this.productsService.getProduct(this.productId).subscribe({
        next: (product) => {
          this.productForm.patchValue({
            name: product.name,
            description: product.description,
            price: product.price
          });
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load product data', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onSubmit() {
    if (this.productForm.valid) {
      this.loading = true;
      const productData = this.productForm.value;

      if (this.isEditMode && this.productId) {
        this.productsService.updateProduct(this.productId, productData).subscribe({
          next: () => {
            this.loading = false;
            this.snackBar.open('Product updated successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/products']);
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to update product', 'Close', { duration: 3000 });
          }
        });
      } else {
        this.productsService.createProduct(productData).subscribe({
          next: () => {
            this.loading = false;
            this.snackBar.open('Product created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/products']);
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to create product', 'Close', { duration: 3000 });
          }
        });
      }
    }
  }
}
