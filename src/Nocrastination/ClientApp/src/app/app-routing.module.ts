import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {
	AccountCreationChildPageComponent,
	AccountCreationParentPagecomponentComponent,
	MainPageComponent,
	LogInPageComponent,
	ChildDashboardComponent,
	ParentDashboardComponent,
	LoginSelectPageComponent,
	AddToSchedulePageComponent,
	CheckOffPageComponent,
	ViewSchedulePageComponent
} from './pages/index';
import { SecurityService, UserService } from './services';

const routes: Routes = [
	{ path: '', redirectTo: '/main', pathMatch: 'full' },
	{ path: 'main', component: MainPageComponent },
	{ path: 'create-child-account', component: AccountCreationChildPageComponent },
	{ path: 'create-parent-account', component: AccountCreationParentPagecomponentComponent },
	{ path: 'log-in', component: LogInPageComponent },
	{ path: 'log-in/:type', component: LogInPageComponent },
	{ path: 'log-in-select', component: LoginSelectPageComponent },
	{ path: 'parent-dashboard', component: ParentDashboardComponent, canActivate: [SecurityService, UserService] },
	{ path: 'child-dashboard', component: ChildDashboardComponent, canActivate: [SecurityService] },

	{ path: 'add-to-shedule', component: AddToSchedulePageComponent, canActivate: [SecurityService, UserService] },
	{ path: 'check-off', component: CheckOffPageComponent, canActivate: [SecurityService, UserService] },
	{ path: 'view-schedule', component: ViewSchedulePageComponent, canActivate: [SecurityService] },

	// { path: 'infopole', component: InfopoleComponent,  },
	// { path: 'infopole/:key/:reportId', component: InfopoleComponent },
	// { path: 'infopole/:key', component: InfopoleComponent,      },

	// { path: '**',  component: NotFoundComponent  },
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [RouterModule]
})
export class AppRoutingModule { }
