import {
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
} from '@angular/material';

import { AccountCreationChildPageComponent,
         AccountCreationParentPagecomponentComponent,
         MainPageComponent,
         LogInPageComponent,
         ParentDashboardComponent,
         ChildDashboardComponent,
         LoginSelectPageComponent,
         AddToSchedulePageComponent,
         CheckOffPageComponent,
         ViewSchedulePageComponent} from './pages/index';

import {
        TaskEditorComponent
 } from './components';

import { SecurityService,
         UserService,
         SchedulerService} from './services';



import { CdkTableModule } from '@angular/cdk/table';
import { RegistrationService } from './services/registration.service';

const MATERIAL_DESIGN = [
    MatAutocompleteModule,
    MatButtonModule,
    MatButtonToggleModule,
    MatCardModule,
    MatCheckboxModule,
    MatChipsModule,
    MatDatepickerModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatGridListModule,
    MatIconModule,
    MatInputModule,
    MatListModule,
    MatMenuModule,
    MatNativeDateModule,
    MatPaginatorModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MatRadioModule,
    MatRippleModule,
    MatSelectModule,
    MatSidenavModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatSnackBarModule,
    MatSortModule,
    MatStepperModule,
    MatTableModule,
    MatTabsModule,
    MatToolbarModule,
    MatTooltipModule,
    CdkTableModule
];

const MODULE_COMPONENTS = [
    AccountCreationChildPageComponent,
    AccountCreationParentPagecomponentComponent,
    MainPageComponent,
    LogInPageComponent,
    ParentDashboardComponent,
    ChildDashboardComponent,
    LoginSelectPageComponent,
    AddToSchedulePageComponent,
    CheckOffPageComponent,
    ViewSchedulePageComponent,

    TaskEditorComponent
];

const MODULE_SERVICES = [
    SecurityService,
    UserService,
    RegistrationService,
    SchedulerService
];

export { MODULE_COMPONENTS, MODULE_SERVICES, MATERIAL_DESIGN };
