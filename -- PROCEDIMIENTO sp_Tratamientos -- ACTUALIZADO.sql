-- PROCEDIMIENTO sp_Tratamientos -- ACTUALIZADO



ALTER PROCEDURE [dbo].[sp_Tratamientos]
(
    @Modo NVARCHAR(20),
    @TratamientoID INT = NULL,
    @MascotaID INT = NULL,
    @EventoID INT = NULL,
    @TipoTratamiento NVARCHAR(50) = NULL,
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
            INSERT INTO tbl_Tratamientos (MascotaID, EventoID, TipoTratamiento, FechaAplicacion, ProximaDosis, Notas, EstadoID)
            VALUES (@MascotaID, @EventoID, @TipoTratamiento, @FechaAplicacion, @ProximaDosis, @Notas, ISNULL(@EstadoID, 1));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Tratamiento agregado correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@TratamientoID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro TratamientoID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_TRATAMIENTO
            BEGIN TRY
                UPDATE tbl_Tratamientos
                SET MascotaID = COALESCE(@MascotaID, MascotaID),
                    EventoID = COALESCE(@EventoID, EventoID),
                    TipoTratamiento = COALESCE(@TipoTratamiento, TipoTratamiento),
                    FechaAplicacion = COALESCE(@FechaAplicacion, FechaAplicacion),
                    ProximaDosis = COALESCE(@ProximaDosis, ProximaDosis),
                    Notas = COALESCE(@Notas, Notas),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE TratamientoID = @TratamientoID;

                COMMIT TRAN UDP_TRATAMIENTO;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Tratamiento actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_TRATAMIENTO;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_Tratamientos WHERE TratamientoID = @TratamientoID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_Tratamientos;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_Tratamientos WHERE TratamientoID = @TratamientoID;
    END
END