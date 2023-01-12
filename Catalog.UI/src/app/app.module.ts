import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { AppRoutes } from './app.routes';
import { CatalogListComponentComponent } from './component/catalog-list-component/catalog-list-component.component';

@NgModule({
  declarations: [
    AppComponent,
    CatalogListComponentComponent
  ],
  imports: [
    BrowserModule,FormsModule,ReactiveFormsModule,RouterModule.forRoot(AppRoutes)
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
