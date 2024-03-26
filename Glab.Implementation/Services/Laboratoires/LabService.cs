using Glab.App.Laboratoires;
using Glab.Domains.Models.Laboratories;
using Glab.Infrastructures.Storages.LaboratoriesStorages;
using GLAB.Domains.Models.Laboratories;
using GLAB.Domains.Shared;
using System.Transactions;

namespace Glab.Implementation.Services.Laboratoires
{
    public class LabService : ILabService

    {
        private readonly ILaboratoryStorage labStorage;

        public LabService(ILaboratoryStorage labStorage)
        {
            this.labStorage = labStorage;
        }


        public async Task<Result> CreateLaboratory(Laboratory laboratory)

        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))

            {

                try

                {

                    List<ErrorCode> errorList = validateLaboratoireForInsert(laboratory); // Corrected method name

                    if (errorList.Any())

                        return Result.Failure(errorList);

                    bool labExists = await labStorage.LaboratoryExistsById(laboratory.LaboratoryId);
                    if (labExists)
                        return Result.Failure(new[] { "A laboratory with this ID already exists." });

                    await labStorage.InsertLaboratory(laboratory);

                    scope.Complete();

                    return Result.Succes;

                }

                catch (Exception ex)

                {

                    // Handle the exception if needed

                    Console.WriteLine($"Error setting laboratory: {ex.Message}");

                    return Result.Failure(new[] { "An error occurred while setting the laboratory." });

                }

            }

        }


        private List<ErrorCode> validateLaboratoireForInsert(Laboratory laboratoire)

        {

            List<ErrorCode> errors = new List<ErrorCode>();

            /*            missing this condition 
             *            if (laboratoire.Status == 0)
                            errors.Add(LaboratoryErrors.StatusBloqued);*/


            if (string.IsNullOrWhiteSpace(laboratoire.Name))

                errors.Add(LaboratoryErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Acronyme))

                errors.Add(LaboratoryErrors.AcronymeEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Adresse))

                errors.Add(LaboratoryErrors.AddressEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.University))

                errors.Add(LaboratoryErrors.UniversityEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Departement))

                errors.Add(LaboratoryErrors.DepartmentEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.PhoneNumber))

                errors.Add(LaboratoryErrors.PhoneNumberEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Email))

                errors.Add(LaboratoryErrors.EmailEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.NumAgrement))

                errors.Add(LaboratoryErrors.AgreementNumberEmpty);

            /*            if (byte.IsNullOrWhiteSpace(laboratoire.Logo)

                            errors.Add(LaboratoryErrors.LogoEmpty);*/


            if (string.IsNullOrWhiteSpace(laboratoire.WebSite))

                errors.Add(LaboratoryErrors.WebSiteEmpty);



            return errors;

        }


        public async Task<List<Laboratory>> GetLaboratories()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                try
                {
                    List<Laboratory> laboratories = await labStorage.SelectLaboratories();


                    if (laboratories == null)
                    {
                        throw new Exception("Laboratories list is null.");
                    }
                    scope.Complete();
                    return laboratories;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting laboratories: {ex.Message}");
                    throw;
                }
        }



        public async Task<Result> SetLaboratory(Laboratory laboratoire)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    List<ErrorCode> errorList = validateLaboratoireForUpdate(laboratoire);
                    if (errorList.Any())
                        return Result.Failure(errorList);

                    await labStorage.UpdateLaboratory(laboratoire);
                    scope.Complete();
                    return Result.Succes;
                }
                catch (Exception ex)
                {
                    // Handle the exception if needed
                    Console.WriteLine($"Error setting laboratory: {ex.Message}");
                    return Result.Failure(new[] { "An error occurred while setting the laboratory." });
                }
            }
        }

        private List<ErrorCode> validateLaboratoireForUpdate(Laboratory laboratoire)
        {
            List<ErrorCode> errors = new List<ErrorCode>();
            /*
                        if (laboratoire.Status == 0)
                            errors.Add(LaboratoryErrors.StatusBloqued);*/

            if (string.IsNullOrWhiteSpace(laboratoire.Name))

                errors.Add(LaboratoryErrors.NameEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Acronyme))

                errors.Add(LaboratoryErrors.AcronymeEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Adresse))

                errors.Add(LaboratoryErrors.AddressEmpty);


            if (string.IsNullOrWhiteSpace(laboratoire.University))

                errors.Add(LaboratoryErrors.UniversityEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Departement))

                errors.Add(LaboratoryErrors.DepartmentEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.PhoneNumber))

                errors.Add(LaboratoryErrors.PhoneNumberEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.Email))

                errors.Add(LaboratoryErrors.EmailEmpty);

            if (string.IsNullOrWhiteSpace(laboratoire.NumAgrement))

                errors.Add(LaboratoryErrors.AgreementNumberEmpty);

            /*            if (byte.IsNullOrWhiteSpace(laboratoire.Logo)

                            errors.Add(LaboratoryErrors.LogoEmpty);*/


            if (string.IsNullOrWhiteSpace(laboratoire.WebSite))

                errors.Add(LaboratoryErrors.WebSiteEmpty);

            return errors;
        }


        public async Task<Result> SetLaboratoryStatus(string id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {

                    await labStorage.UpdateLaboratoryStatus(id);

                    scope.Complete();
                    return Result.Succes;
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a failure result with the exception message
                return Result.Failure(new[] { ex.Message });
            }
        }

        public async Task<Laboratory> GetLaboratoryById(string id)
        {
            bool labExists = await labStorage.LaboratoryExistsById(id);
            if (!labExists)
            {
                // Handle the case where the laboratory with the specified ID does not exist
                throw new ArgumentException("Laboratory with the specified ID does not exist.");
            }
            return await labStorage.SelectLaboratoryById(id);
        }

        public async Task<Laboratory> GetLaboratoryByName(string name)
        {
            bool labExists = await labStorage.LaboratoryExistsByName(name);
            if (!labExists)
            {
                // Handle the case where the laboratory with the specified name does not exist
                throw new ArgumentException("Laboratory with the specified name does not exist.");
            }
            return await labStorage.SelectLaboratoryByName(name);
        }
    }





    /* Unuseable Methods */
    /*        private async Task<bool> laboratoryIsValid(Laboratory laboratory)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(laboratory.LaboratoryId))
                        throw new ArgumentException("LaboratoryId cannot be null or whitespace.", nameof(laboratory.LaboratoryId));
                    if (string.IsNullOrWhiteSpace(laboratory.Name))
                        throw new ArgumentException("Name cannot be null or whitespace.", nameof(laboratory.Name));
                    if (string.IsNullOrWhiteSpace(laboratory.Acronyme))
                        throw new ArgumentException("Acronyme cannot be null or whitespace.", nameof(laboratory.Acronyme));
                    if (string.IsNullOrWhiteSpace(laboratory.Adresse))
                        throw new ArgumentException("Adresse cannot be null or whitespace.", nameof(laboratory.Adresse));
                    if (string.IsNullOrWhiteSpace(laboratory.Email))
                        throw new ArgumentException("Email cannot be null or whitespace.", nameof(laboratory.Email));

                    *//*  if (await LaboratoireExists(laboratory.LaboratoryId))
                          throw new InvalidOperationException($"Laboratoire with ID '{laboratory.LaboratoryId}' already exists.");
                    *//*
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error validating laboratoire: {ex.Message}");
                    return false;
                }
            }


            private static async Task<bool> validateId(string id)
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentException($"Invalid ID: {id}", nameof(id));
                }

                return true;
            }*/


}