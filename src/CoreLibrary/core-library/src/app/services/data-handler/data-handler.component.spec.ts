import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DataHandlerComponent } from './data-handler.component';

describe('DataHandlerComponent', () => {
  let component: DataHandlerComponent;
  let fixture: ComponentFixture<DataHandlerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DataHandlerComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DataHandlerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
