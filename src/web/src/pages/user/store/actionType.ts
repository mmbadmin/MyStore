export enum UserActionTypes {
    // data
    UserTableLoading = "@@User/UserTableLoading",
    UserSetTableData = "@@User/UserSetTableData",
    UserSetItemData = "@@User/UserSetItemData",
    // Create
    UserCreateLoading = "@@User/UserCreateLoading",
    UserCreateModal = "@@User/UserCreateModal",
    // Update
    UserUpdateLoading = "@@User/UserUpdateLoading",
    UserUpdateModal = "@@User/UserUpdateModal",
    UserUpdateSetItemID = "@@User/UserUpdateSetItemID",
    // Roles
    UserRoleSetRoles = "@@User/UserSetRoles",
    UserRoleModal = "@@User/UserRoleModal",
    UserRoleSetData = "@@User/UserSetData",
    UserRoleSetLoading = "@@User/UserRoleSetLoading",
    // change password
    UserPasswordSetLoading = "@@User/UserPasswordSetLoading",
    UserPasswordSetItemId = "@@User/UserPasswordSetItemId",
    UserPasswordModal = "@@User/UserPasswordModal",
}
