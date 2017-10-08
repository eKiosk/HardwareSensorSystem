import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
  MdButtonModule,
  MdCardModule,
  MdInputModule,
  MdListModule,
  MdSidenavModule,
  MdToolbarModule,
} from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  imports: [
    BrowserAnimationsModule,
    FlexLayoutModule,
    MdSidenavModule,
    MdToolbarModule,
    MdButtonModule,
    MdInputModule,
    MdCardModule,
    MdListModule
  ],
  exports: [
    BrowserAnimationsModule,
    FlexLayoutModule,
    MdSidenavModule,
    MdToolbarModule,
    MdButtonModule,
    MdInputModule,
    MdCardModule,
    MdListModule
  ]
})
export class UiComponentsModule { }
