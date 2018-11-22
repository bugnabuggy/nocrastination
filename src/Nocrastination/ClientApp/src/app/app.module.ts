import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NocrastinationHttpInterceptor } from './interceptors/nocrastination-http.interceptor';

import { OwlDateTimeModule, OwlNativeDateTimeModule } from 'ng-pick-datetime';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';


import { AppComponent } from './app.component';

import { MODULE_COMPONENTS, MODULE_SERVICES, MATERIAL_DESIGN } from './moduleExports';

@NgModule({
  declarations: [
    AppComponent,
    MODULE_COMPONENTS,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    FormsModule,
    MATERIAL_DESIGN,
    BrowserAnimationsModule,
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
    CalendarModule.forRoot({
      provide: DateAdapter,
      useFactory: adapterFactory
    })
  ],
  providers: [
    MODULE_SERVICES,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: NocrastinationHttpInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
