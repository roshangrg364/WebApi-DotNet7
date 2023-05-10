using CoreModule.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Src
{
    public class VillaNumberService : VillaNumberServiceInterface
    {
        private readonly UnitOfWorkServiceInterface _unitOfWork;
        public VillaNumberService(UnitOfWorkServiceInterface unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<VillaNumberResponseDto> Create(VillaNumberCreateDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                if (string.IsNullOrEmpty(dto.VillaNo)) throw new CustomException("Villa Number is required");
                await ValidateDupliateVillaNo(dto.VillaNo);
                var villa = await _unitOfWork.Villas.GetByIdAsync(dto.VillaId).ConfigureAwait(false) ?? throw new CustomException("Villa not found");
                var villaNumber = new VillaNumber(dto.VillaNo,villa)
                {
                    Details = dto.Details,
                };
                await _unitOfWork.VillaNumbers.AddAsync(villaNumber);
                await _unitOfWork.CompleteAsync();
                await tx.CommitAsync();
                var response = new VillaNumberResponseDto()
                {
                    Id = villaNumber.Id,
                    VillaNo = villaNumber.VillaNo,
                    Details = villaNumber.Details,
                    Villa = new VillaResponseDto
                    {
                        Id = villa.Id,
                        Name = villa.Name,
                        Area = villa.Area,
                        Details = villa.Details,
                        Rate = villa.Rate,
                        Occupancy = villa.Occupancy,
                        Amenity = villa.Amenity,
                        ImageUrl = villa.ImageUrl

                    }
                };
                return response;
            }



        }

        public async Task Delete(int id)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var villaNumber = await _unitOfWork.VillaNumbers.GetByIdAsync(id).ConfigureAwait(false) ?? throw new CustomException($"VillaNumber not found for id {id}");
                _unitOfWork.VillaNumbers.Delete(villaNumber);
                await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                tx.Commit();
            }
        }

        public async Task<List<VillaNumberResponseDto>> GetAllVillas()
        {
            var villaNumbers = await _unitOfWork.VillaNumbers.GetQueryable().AsNoTracking().Include(a=>a.Villa).ToListAsync();
            var returnModel = new List<VillaNumberResponseDto>();
            foreach (var villaNumber in villaNumbers)
            {
                returnModel.Add(new VillaNumberResponseDto
                {
                    Id = villaNumber.Id,
                    Details = villaNumber.Details,
                    VillaNo = villaNumber.VillaNo,
                    VillaId = villaNumber.VillaId,
                    Villa = new VillaResponseDto
                    {
                        Id = villaNumber.Villa.Id,
                        Name = villaNumber.Villa.Name,
                        Area = villaNumber.Villa.Area,
                        Details = villaNumber.Villa.Details,
                        Rate = villaNumber.Villa.Rate,
                        Occupancy = villaNumber.Villa.Occupancy,
                        Amenity = villaNumber.Villa.Amenity,
                        ImageUrl = villaNumber.Villa.ImageUrl

                    }

                });
            }
            return returnModel;
        }

        public async Task<VillaNumberResponseDto> GetById(int id)
        {
            var villaNumber = await _unitOfWork.VillaNumbers.GetQueryable().Include(a=>a.Villa).FirstOrDefaultAsync(b=>b.Id == id) ?? throw new CustomException("Villa Number not found");
            return new VillaNumberResponseDto
            {
                Id = villaNumber.Id,
                Details = villaNumber.Details,
               VillaNo = villaNumber.VillaNo,
               VillaId = villaNumber.VillaId,
               Villa = new VillaResponseDto
               {
                   Id = villaNumber.Villa.Id,
                   Name = villaNumber.Villa.Name,
                   Area = villaNumber.Villa.Area,
                   Details = villaNumber.Villa.Details,
                   Rate = villaNumber.Villa.Rate,
                   Occupancy = villaNumber.Villa.Occupancy,
                   Amenity = villaNumber.Villa.Amenity,
                   ImageUrl = villaNumber.Villa.ImageUrl

               }
            };

        }

        public async Task<VillaNumberResponseDto> Update(VillaNumberUpdateDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                var villaNumber = await _unitOfWork.VillaNumbers.GetByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new CustomException("Villa Not Found");
                if (string.IsNullOrEmpty(dto.VillaNo)) throw new CustomException("Villa Number  is required");
                villaNumber.Update(dto.VillaNo);
                villaNumber.Details = dto.Details;
                await _unitOfWork.CompleteAsync();
                await tx.CommitAsync();
                return new VillaNumberResponseDto
                {
                    Id = villaNumber.Id,
                    Details = villaNumber.Details,
                    VillaNo = villaNumber.VillaNo
                };
            }
        }

        private async Task ValidateDupliateVillaNo(string villaNo,VillaNumber villaNumber =null)
        {
            var villaWithSameVillaNo = await _unitOfWork.VillaNumbers.GetQueryable().AsNoTracking().FirstOrDefaultAsync(a => a.VillaNo == villaNo);
            if (villaWithSameVillaNo != null && villaWithSameVillaNo != villaNumber) throw new CustomException("Villa No already exists");
        }
    }
}

