import { Routes } from "@angular/router";
import { CatalogListComponentComponent } from "./component/catalog-list-component/catalog-list-component.component";

export const AppRoutes :Routes =[
    {path:'list-catalog',component:CatalogListComponentComponent},
    {path:'**',redirectTo:'list-catalog'}
];