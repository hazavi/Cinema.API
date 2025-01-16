using AutoMapper;
using DAL.Models.Domain;
using DAL.Models.DTO;

namespace NZWalks.API.Mappings
{
    // konvertere data mellem forskellige objekttyper
    // bruges til at mappe (kopiere) data fra en klasse til en anden, især mellem Domain Models og Data Transfer Objects (DTO'er).
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Users
            CreateMap<AddUserRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // Ignore PasswordHash since it's handled in repository
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())  // Ignore PasswordSalt as well
                .ForMember(dest => dest.CreateDate, opt => opt.Ignore())  // Ignore CreateDate as it's auto-generated
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin));  // Ensure IsAdmin is mapped

            CreateMap<UpdateUserRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())  // Ignore PasswordHash since it's handled in repository
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())  // Ignore PasswordSalt as well
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin));  // Ensure IsAdmin is mapped

            // Map User to UserDto (no need to map PasswordHash and PasswordSalt)
            CreateMap<User, UserDto>()
                .ReverseMap();



            // PostalCode
            CreateMap<PostalCode, PostalCodeDto>().ReverseMap();
            CreateMap<AddPostalCodeRequestDto, PostalCode>().ReverseMap();
            CreateMap<UpdatePostalCodeRequestDto, PostalCode>().ReverseMap();

            // Genre
            CreateMap<AddGenreRequestDto, Genre>()
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.GenreName))
                .ForMember(dest => dest.MovieGenres, opt => opt.Ignore());
            CreateMap<Genre, GenreDto>().ReverseMap();

            // Movie
            CreateMap<AddMovieRequestDto, Movie>()
                    .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src =>
                        src.GenreIds != null
                            ? src.GenreIds.Select(gid => new MovieGenre { GenreId = gid }).ToList()
                            : new List<MovieGenre>()))
                    .ForMember(dest => dest.isShowing, opt => opt.MapFrom(src => src.isShowing)) 
                    .ForMember(dest => dest.ReleaseDate, opt => opt.MapFrom(src => src.ReleaseDate));



            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.PosterUrl, opt => opt.MapFrom(src => src.PosterUrl))
                .ForMember(dest => dest.MovieGenres, opt => opt.MapFrom(src =>
                    src.MovieGenres.Select(mg => new MovieGenreDto
                    {
                        GenreId = mg.GenreId,
                        GenreName = mg.Genre.GenreName
                    }).ToList()))
                .ForMember(dest => dest.isShowing, opt => opt.MapFrom(src => src.isShowing));


            CreateMap<MovieGenre, MovieGenreDto>()
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.GenreId, opt => opt.MapFrom(src => src.GenreId))
                .ForMember(dest => dest.GenreName, opt => opt.MapFrom(src => src.Genre.GenreName));

            CreateMap<UpdateMovieRequestDto, Movie>();

            // Address
            CreateMap<AddAddressRequestDto, Address>().ReverseMap();
            CreateMap<UpdateAddressRequestDto, Address>().ReverseMap();
            CreateMap<Address, AddressDto>().ReverseMap();

            // Theater
            CreateMap<AddTheaterRequestDto, Theater>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location));
            // Mapping from Theater to TheaterDto and vice versa
            CreateMap<Theater, TheaterDto>().ReverseMap();


            // Seat
            CreateMap<Seat, SeatDto>().ReverseMap();
            // AddSeatRequestDto to Seat mapping
            CreateMap<AddSeatRequestDto, Seat>();

            // UpdateSeatRequestDto to Seat mapping
            CreateMap<UpdateSeatRequestDto, Seat>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Showtime
            CreateMap<AddShowtimeRequestDto, Showtime>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.MovieId))
                .ForMember(dest => dest.TheaterId, opt => opt.MapFrom(src => src.TheaterId));
            CreateMap<Showtime, ShowtimeDto>().ReverseMap();


            // Ticket
            CreateMap<Ticket, TicketDto>().ReverseMap();
        }
    }
}
