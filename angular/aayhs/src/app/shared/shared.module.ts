import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HeaderComponent } from '../shared/layout/header/header/header.component';
import { SidebarComponent } from '../shared/layout/sidebar/sidebar/sidebar.component';
import { FooterComponent } from '../shared/layout/footer/footer/footer.component';
import { RouterModule } from '@angular/router';

//All material imports here//
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CdkStepperModule } from '@angular/cdk/stepper';
import { MatStepperModule } from '@angular/material/stepper';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CdkTableModule } from '@angular/cdk/table';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';

import { MatTabsModule } from '@angular/material/tabs';



// All third party imports here //
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackbarComponent } from './ui/mat-snackbar/mat-snackbar/mat-snackbar.component';
import { ConfirmDialogComponent } from './ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';



@NgModule({
  declarations: [HeaderComponent, FooterComponent,SidebarComponent, MatSnackbarComponent, ConfirmDialogComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    // All material controls imports here //
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CdkStepperModule,
    MatStepperModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    CdkTableModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    // All third party imports here //
    
    MatSelectModule
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    SidebarComponent,

    RouterModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    //All material exports here//
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    CdkStepperModule,
    MatStepperModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatDialogModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    CdkTableModule,
    MatTabsModule,
    MatTableModule,
    MatPaginatorModule,
    // All third party exports here //
  
    MatSelectModule
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  providers: [
  
  ]
})
export class SharedModule { }
