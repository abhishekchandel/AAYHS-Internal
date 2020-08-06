import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportConfirmationModalComponent } from './export-confirmation-modal.component';

describe('ExportConfirmationModalComponent', () => {
  let component: ExportConfirmationModalComponent;
  let fixture: ComponentFixture<ExportConfirmationModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportConfirmationModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportConfirmationModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
