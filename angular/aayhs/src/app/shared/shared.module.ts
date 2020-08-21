import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HeaderComponent } from '../shared/layout/header/header/header.component';
import { SidebarComponent } from '../shared/layout/sidebar/sidebar/sidebar.component';
import { FooterComponent } from '../shared/layout/footer/footer/footer.component';
import { RouterModule } from '@angular/router';
import { ConfirmEqualValidatorDirective } from './directives/confirm-equal-validator.directive';
import {SearchPipe} from '../shared/filters/search.pipe'
import { AssignStallModalComponent } from './ui/modals/assign-stall-modal/assign-stall-modal.component';
import { FinancialTransactionsComponent } from './ui/modals/financial-transactions/financial-transactions.component';

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
import { NgxMaskModule, IConfig } from 'ngx-mask';
import { AddSplitClassModalComponent } from './ui/modals/add-split-class-modal/add-split-class-modal.component'
import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';
import { ExportConfirmationModalComponent } from './ui/modals/export-confirmation-modal/export-confirmation-modal.component';
import { OrderModule } from 'ngx-order-pipe';
import { ExportAsModule } from 'ngx-export-as';
import {NgxPrintModule} from 'ngx-print';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

export var options: Partial<IConfig> | (() => Partial<IConfig>);
const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

@NgModule({
  declarations: [HeaderComponent, FooterComponent,SidebarComponent, MatSnackbarComponent, ConfirmDialogComponent, AddSplitClassModalComponent, ExportConfirmationModalComponent, ConfirmEqualValidatorDirective, SearchPipe, AssignStallModalComponent, FinancialTransactionsComponent],
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
    MatSnackBarModule,
    // All third party imports here //
    NgxMaskModule.forRoot(options),
    MatSelectModule,
    PerfectScrollbarModule,
    OrderModule,
    ExportAsModule,
    NgxPrintModule,
    BsDatepickerModule.forRoot()
  ],
  exports: [
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    MatSnackbarComponent,
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
    MatSnackBarModule,
    // All third party exports here //
    NgxMaskModule,
    MatSelectModule,
    PerfectScrollbarModule,
    OrderModule,
    ExportAsModule,
    NgxPrintModule,
    ConfirmEqualValidatorDirective,
    SearchPipe,
    BsDatepickerModule
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA
  ],
  providers: [
    MatSnackbarComponent,
      
  ]
})
export class SharedModule { }
