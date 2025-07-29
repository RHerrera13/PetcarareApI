-- PROCEDIMIENTO sp_RegistroMedico -- ACTUALIZADO

ALTER PROCEDURE [dbo].[sp_RegistrosMedicos]
(
    @Modo NVARCHAR(20),
    @RegistroID INT = NULL,
    @EventoID INT = NULL,
    @TipoTratamiento NVARCHAR(50) = NULL,
    @VeterinarioNombre NVARCHAR(100) = NULL,
    @Descripcion NVARCHAR(MAX) = NULL,
    @EstadoID INT = NULL,
    @O_Numero INT = NULL OUTPUT,
    @O_Mensage VARCHAR(255) = NULL OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @Modo = 'AGREGAR'
    BEGIN
        BEGIN TRY
            INSERT INTO tbl_RegistrosMedicos (EventoID, TipoTratamiento, VeterinarioNombre, Descripcion, EstadoID)
            VALUES (@EventoID, @TipoTratamiento, @VeterinarioNombre, @Descripcion, ISNULL(@EstadoID, 1));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Registro médico agregado correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@RegistroID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro RegistroID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_REGMED
            BEGIN TRY
                UPDATE tbl_RegistrosMedicos
                SET EventoID = COALESCE(@EventoID, EventoID),
                    TipoTratamiento = COALESCE(@TipoTratamiento, TipoTratamiento),
                    VeterinarioNombre = COALESCE(@VeterinarioNombre, VeterinarioNombre),
                    Descripcion = COALESCE(@Descripcion, Descripcion),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE RegistroID = @RegistroID;

                COMMIT TRAN UDP_REGMED;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Registro médico actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_REGMED;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_RegistrosMedicos WHERE RegistroID = @RegistroID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_RegistrosMedicos;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_RegistrosMedicos WHERE RegistroID = @RegistroID;
    END
END


EXEC sp_RegistrosMedicos
@MODO = 'LISTAR'