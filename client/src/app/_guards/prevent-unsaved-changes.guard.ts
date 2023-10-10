import { Injectable } from '@angular/core';
import { CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  canDeactivate(component: MemberEditComponent,//which component we are trying to deactivate
    ):  boolean {
    if(component.editForm?.dirty){
      return confirm('are you sure you want to continue ? Any unsaved changes will be lost');
    }
    return true;
  }
  
}
