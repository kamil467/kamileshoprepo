import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CatalogListComponentComponent } from './catalog-list-component.component';

describe('CatalogListComponentComponent', () => {
  let component: CatalogListComponentComponent;
  let fixture: ComponentFixture<CatalogListComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CatalogListComponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CatalogListComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
