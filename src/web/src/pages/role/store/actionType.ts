export enum RoleActionTypes {
    // data
    RoleTableLoading = "@@Role/RoleTableLoading",
    RoleSetTableData = "@@Role/RoleSetTableData",
    RoleSetItemData = "@@Role/RoleSetItemData",
    // Create
    RoleCreateLoading = "@@Role/RoleCreateLoading",
    RoleCreateModal = "@@Role/RoleCreateModal",
    // Update
    RoleUpdateLoading = "@@Role/RoleUpdateLoading",
    RoleUpdateModal = "@@Role/RoleUpdateModal",
    RoleUpdateSetItemID = "@@Role/RoleUpdateSetItemID",
    // Permission
    RolePermissionModal = "@@Role/RolePermission",
    RolePermissionSetPermissionData = "@@Role/RolePermissionSetPermissionData",
    RolePermissionSetRoleData = "@@Role/RolePermissionSetRoleData",
    RolePermissionLoading = "@@Role/RolePermissionLoading",
}
