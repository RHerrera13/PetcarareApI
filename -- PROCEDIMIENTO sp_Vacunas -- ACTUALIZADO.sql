-- PROCEDIMIENTO sp_Vacunas -- ACTUALIZADO

ALTER PROCEDURE [dbo].[sp_Vacunas]
(
    @Modo NVARCHAR(20),
    @VacunaID INT = NULL,
    @MascotaID INT = NULL,
    @RegistroID INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @FechaAplicacion DATE = NULL,
    @ProximaDosis DATE = NULL,
    @Notas NVARCHAR(MAX) = NULL,
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
            INSERT INTO tbl_Vacunas (MascotaID, RegistroID, Nombre, FechaAplicacion, ProximaDosis, Notas, EstadoID)
            VALUES (@MascotaID, @RegistroID, @Nombre, @FechaAplicacion, @ProximaDosis, @Notas, ISNULL(@EstadoID, 1));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Vacuna agregada correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@VacunaID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro VacunaID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_VACUNA
            BEGIN TRY
                UPDATE tbl_Vacunas
                SET MascotaID = COALESCE(@MascotaID, MascotaID),
                    RegistroID = COALESCE(@RegistroID, RegistroID),
                    Nombre = COALESCE(@Nombre, Nombre),
                    FechaAplicacion = COALESCE(@FechaAplicacion, FechaAplicacion),
                    ProximaDosis = COALESCE(@ProximaDosis, ProximaDosis),
                    Notas = COALESCE(@Notas, Notas),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE VacunaID = @VacunaID;

                COMMIT TRAN UDP_VACUNA;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Vacuna actualizada correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_VACUNA;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_Vacunas WHERE VacunaID = @VacunaID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_Vacunas;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_Vacunas WHERE VacunaID = @VacunaID;
    END
END
