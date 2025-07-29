-- PROCEDIMIENTO: sp_Usuarios // Actualizado a las ultimas mejoras 


ALTER PROCEDURE sp_Usuarios
    @Modo NVARCHAR(20),
    @UsuarioID INT = NULL,
    @NombreCompleto NVARCHAR(100) = NULL,
    @Email NVARCHAR(100) = NULL,
    @Telefono NVARCHAR(20) = NULL,
    @Contrasena NVARCHAR(256) = NULL,
    @FechaRegistro DATETIME = NULL,
    @EstadoID INT = NULL,
    @O_Numero INT = NULL OUTPUT,
    @O_Mensage VARCHAR(255) = NULL OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @Modo = 'AGREGAR'
    BEGIN
        INSERT INTO tbl_Usuarios (NombreCompleto, Email, Telefono, Contrasena, FechaRegistro, EstadoID)
        VALUES (@NombreCompleto, @Email, @Telefono, CONVERT(VARBINARY(256), @Contrasena), ISNULL(@FechaRegistro, GETDATE()), ISNULL(@EstadoID, 1));
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@UsuarioID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro UsuarioID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_USUARIO;
            BEGIN TRY
                UPDATE tbl_Usuarios
                SET NombreCompleto = COALESCE(@NombreCompleto, NombreCompleto),
                    Email = COALESCE(@Email, Email),
                    Telefono = COALESCE(@Telefono, Telefono),
                    Contrasena = COALESCE(CONVERT(VARBINARY(256), @Contrasena), Contrasena),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE UsuarioID = @UsuarioID;

                COMMIT TRAN UDP_USUARIO;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Usuario actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_USUARIO;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_Usuarios WHERE UsuarioID = @UsuarioID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_Usuarios;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_Usuarios WHERE UsuarioID = @UsuarioID;
    END
END
GO
