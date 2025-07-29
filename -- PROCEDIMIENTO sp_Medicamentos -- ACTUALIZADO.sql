-- PROCEDIMIENTO sp_Medicamentos -- ACTUALIZADO


ALTER PROCEDURE [dbo].[sp_Medicamentos]
(
    @Modo NVARCHAR(20),
    @MedicamentoID INT = NULL,
    @MascotaID INT = NULL,
    @RegistroID INT = NULL,
    @Nombre NVARCHAR(100) = NULL,
    @Dosis NVARCHAR(100) = NULL,
    @Frecuencia NVARCHAR(100) = NULL,
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL,
    @DosisTomadas INT = NULL,
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
            INSERT INTO tbl_Medicamentos (MascotaID, RegistroID, Nombre, Dosis, Frecuencia, FechaInicio, FechaFin, DosisTomadas, Notas, EstadoID)
            VALUES (@MascotaID, @RegistroID, @Nombre, @Dosis, @Frecuencia, @FechaInicio, @FechaFin, 
                    ISNULL(@DosisTomadas, 0), @Notas, ISNULL(@EstadoID, 1));

            SET @O_Numero = 0;
            SET @O_Mensage = 'Medicamento agregado correctamente';
        END TRY
        BEGIN CATCH
            SET @O_Numero = ERROR_NUMBER();
            SET @O_Mensage = ERROR_MESSAGE();
        END CATCH
    END
    ELSE IF @Modo = 'ACTUALIZAR'
    BEGIN
        IF ISNULL(@MedicamentoID, 0) = 0
        BEGIN
            SET @O_Numero = -1;
            SET @O_Mensage = 'El parametro MedicamentoID no puede ser nulo';
        END
        ELSE
        BEGIN
            BEGIN TRAN UDP_MEDICAMENTO
            BEGIN TRY
                UPDATE tbl_Medicamentos
                SET MascotaID = COALESCE(@MascotaID, MascotaID),
                    RegistroID = COALESCE(@RegistroID, RegistroID),
                    Nombre = COALESCE(@Nombre, Nombre),
                    Dosis = COALESCE(@Dosis, Dosis),
                    Frecuencia = COALESCE(@Frecuencia, Frecuencia),
                    FechaInicio = COALESCE(@FechaInicio, FechaInicio),
                    FechaFin = COALESCE(@FechaFin, FechaFin),
                    DosisTomadas = COALESCE(@DosisTomadas, DosisTomadas),
                    Notas = COALESCE(@Notas, Notas),
                    EstadoID = COALESCE(@EstadoID, EstadoID)
                WHERE MedicamentoID = @MedicamentoID;

                COMMIT TRAN UDP_MEDICAMENTO;
                SET @O_Numero = 0;
                SET @O_Mensage = 'Medicamento actualizado correctamente';
            END TRY
            BEGIN CATCH
                ROLLBACK TRAN UDP_MEDICAMENTO;
                SET @O_Numero = ERROR_NUMBER();
                SET @O_Mensage = ERROR_MESSAGE();
            END CATCH
        END
    END
    ELSE IF @Modo = 'ELIMINAR'
    BEGIN
        DELETE FROM tbl_Medicamentos WHERE MedicamentoID = @MedicamentoID;
    END
    ELSE IF @Modo = 'LISTAR'
    BEGIN
        SELECT * FROM tbl_Medicamentos;
    END
    ELSE IF @Modo = 'BUSCAR'
    BEGIN
        SELECT * FROM tbl_Medicamentos WHERE MedicamentoID = @MedicamentoID;
    END
END