using CoreModule.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaService : VillaServiceInterface
    {
        private readonly UnitOfWorkServiceInterface _unitOfWork;
        public VillaService(UnitOfWorkServiceInterface unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<VillaResponseDto> Create(VillaCreateDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                if (string.IsNullOrEmpty(dto.Name)) throw new CustomException("Villa Name is required");
                var villa = new Villa(dto.Name)
                {
                    Area = dto.Area,
                    Details = dto.Details,
                    Rate = dto.Rate,
                    Occupancy = dto.Occupancy,
                    Amenity = dto.Amenity,
                    ImageUrl = dto.ImageUrl
                };
                await _unitOfWork.Villas.AddAsync(villa);
                await _unitOfWork.CompleteAsync();
                await tx.CommitAsync();
                var response = new VillaResponseDto() {
                    Id = villa.Id,
                    Name = villa.Name,
                    Area = villa.Area,
                    Details = villa.Details,
                    Rate = villa.Rate,
                    Occupancy = villa.Occupancy,
                    Amenity = villa.Amenity,
                    ImageUrl=villa.ImageUrl
                };
                return response;
            }



        }

        public async Task Delete(int id)
        {
            using(var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var villa = await _unitOfWork.Villas.GetByIdAsync(id).ConfigureAwait(false) ?? throw new CustomException($"Villa not found for id {id}");
                 _unitOfWork.Villas.Delete(villa);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                tx.Commit();
            }
        }

        public async Task<List<VillaResponseDto>> GetAllVillas()
        {
            var villas = await _unitOfWork.Villas.GetAllAsync();
            var returnModel = new List<VillaResponseDto>();
           foreach(var villa in villas)
            {
                returnModel.Add(new VillaResponseDto
                {
                    Id = villa.Id,
                    Name = villa.Name,
                    Area = villa.Area,
                    Details = villa.Details,
                    Rate = villa.Rate,
                    Occupancy = villa.Occupancy,
                    Amenity = villa.Amenity,
                    ImageUrl = villa.ImageUrl

                });
            }
            return returnModel;

        }

        public async Task<VillaResponseDto> GetById(int id)
        {
            var villa = await _unitOfWork.Villas.GetByIdAsync(id) ?? throw new CustomException("Villa not found");
            return new VillaResponseDto {
                Id = villa.Id,
                Name = villa.Name,
                Area = villa.Area,
                Details = villa.Details,
                Rate = villa.Rate,
                Occupancy = villa.Occupancy,
                Amenity = villa.Amenity,
                ImageUrl = villa.ImageUrl
            };

        }

        public async Task<VillaResponseDto> Update(VillaUpdateDto dto)
        {
            using(var tx= await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var villa =await _unitOfWork.Villas.GetByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new CustomException("Villa Not Found");
                if (string.IsNullOrEmpty(dto.Name)) throw new CustomException("Villa Name is required");
                villa.Update(dto.Name);
                villa.Area = dto.Area;
                villa.Rate = dto.Rate;
                villa.Details = dto.Details;
                villa.Occupancy = dto.Occupancy;
                villa.Amenity = dto.Amenity;
                villa.ImageUrl = dto.ImageUrl;
               await _unitOfWork.CompleteAsync();
                await tx.CommitAsync();
                return new VillaResponseDto
                {
                    Id = villa.Id,
                    Name = villa.Name,
                    Area = villa.Area,
                    Details = villa.Details,
                    Rate = villa.Rate,
                    Occupancy = villa.Occupancy,
                    Amenity = villa.Amenity,
                    ImageUrl = villa.ImageUrl
                };
            }
        }
    }
}
