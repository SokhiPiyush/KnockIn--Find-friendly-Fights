import { Injectable } from '@angular/core';
import { CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service';

@Injectable({
  providedIn: 'root'
})
export class PreventUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  constructor(private confirmService: ConfirmService){
     
  }

  canDeactivate(component: MemberEditComponent,//which component we are trying to deactivate
    ):  Observable<boolean> {
    if(component.editForm?.dirty){
      // return confirm('are you sure you want to continue ? Any unsaved changes will be lost');
      return this.confirmService.confirm()
    }
    return of(true);
  }
  
}
