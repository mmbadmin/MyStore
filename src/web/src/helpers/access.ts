class Permission {
    private access: Partial<IAccessModel> = {};

    public set Access(value: Partial<IAccessModel>) {
        this.access = value;
    }

    public get Access(): Partial<IAccessModel> {
        return this.access;
    }
}

export interface IAccessModel {
    role_RoleCreateCommandHandler: "string",
    role_RoleDeleteCommandHandler: "string",
    role_RoleGetItemQueryHandler: "string",
    role_RoleGetPagedListQueryHandler: "string",
    role_RolePermissionUpdateCommandHandler: "string",
    role_RoleUpdateCommandHandler: "string",
    user_UserActiveCommandHandler: "string",
    user_UserCreateCommandHandler: "string",
    user_UserDeactiveCommandHandler: "string",
    user_UserDeleteCommandHandler: "string",
    user_UserGetItemQueryHandler: "string",
    user_UserGetPagedListQueryHandler: "string",
    user_UserGetRoleQueryHandler: "string",
    user_UserResetPasswordCommandHandler: "string",
    user_UserRoleUpdateCommandHandler: "string",
    user_UserUnlockCommandHandler: "string",
    user_UserUpdateCommandHandler: "string",
    sladbForm_SLADBFormGetPagedListQueryHandler: "string",
    sladbForm_SLADBFormDeleteCommandHandler: "string",
    sladbForm_SLADBFormCreateCommandHandler: "string",
    sladbForm_SLADBFormUpdateCommandHandler: "string",
}
const permission = new Permission();

export { permission };
