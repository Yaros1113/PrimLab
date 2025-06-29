import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ClientsService } from '../../services/clients.service';
import { Client } from '../../models/client.model';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-client-form',
  templateUrl: './client-form.component.html',
  styleUrls: ['./client-form.component.css']
})
export class ClientFormComponent implements OnInit {
  clientForm: FormGroup;
  isEditMode = false;
  clientId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private clientsService: ClientsService,
    private route: ActivatedRoute,
    public router: Router,
    private snackBar: MatSnackBar
  ) {
    this.clientForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumbers: this.fb.array([this.fb.control('', Validators.required)])
    });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.clientId = +params['id'];
        this.loadClientData();
      }
    });
  }

  get phoneNumbers() {
    return this.clientForm.get('phoneNumbers') as FormArray;
  }

  addPhoneNumber() {
    this.phoneNumbers.push(this.fb.control('', Validators.required));
  }

  removePhoneNumber(index: number) {
    this.phoneNumbers.removeAt(index);
  }

  loadClientData() {
    if (this.clientId) {
      this.loading = true;
      this.clientsService.getClient(this.clientId).subscribe({
        next: (client) => {
          this.clientForm.patchValue({
            name: client.name,
            email: client.email
          });
          client.phoneNumbers.forEach(phone => {
            this.phoneNumbers.push(this.fb.control(phone, Validators.required));
          });
          this.phoneNumbers.removeAt(0);
          this.loading = false;
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Failed to load client data', 'Close', { duration: 3000 });
        }
      });
    }
  }

  onSubmit() {
    if (this.clientForm.valid) {
      this.loading = true;
      const clientData = this.clientForm.value;

      if (this.isEditMode && this.clientId) {
        this.clientsService.updateClient(this.clientId, clientData).subscribe({
          next: () => {
            this.loading = false;
            this.snackBar.open('Client updated successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/clients']);
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to update client', 'Close', { duration: 3000 });
          }
        });
      } else {
        this.clientsService.createClient(clientData).subscribe({
          next: () => {
            this.loading = false;
            this.snackBar.open('Client created successfully', 'Close', { duration: 3000 });
            this.router.navigate(['/clients']);
          },
          error: () => {
            this.loading = false;
            this.snackBar.open('Failed to create client', 'Close', { duration: 3000 });
          }
        });
      }
    }
  }
}
