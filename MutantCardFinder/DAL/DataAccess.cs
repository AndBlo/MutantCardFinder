using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MutantCardFinder.DataContext;
using MutantCardFinder.Model;
using LiteDB;

namespace MutantCardFinder.DAL
{
    class DataAccess
    {

        public void InsertTestTalents()
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");

                var talent1 = new Talent()
                {
                    Name = "Hård jävel",
                    Description = "Spelaren tål mer och ger hårdare slag",
                    GameMechanics = "+1 i STY-relaterade kast"
                };

                var talent2 = new Talent()
                {
                    Name = "Uppfinnare",
                    Description = "Spelaren kan förvandla vilket skrot som helst till någonting användbart",
                    GameMechanics = "+2 i SKP-relaterade kast"
                };

                var value = talents.Insert(talent1);

                MessageBox.Show(value.AsInt32.ToString());

                MessageBox.Show(talents.Insert(talent2));

                talents.EnsureIndex(p => p.Name);
            }
        }

        public Talent GetTalent(string name)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");

                var result = talents.Find(p => p.Name.ToLower().Contains(name.ToLower()));

                return result.FirstOrDefault();
            }
        }

        public void UpsertTalent(TalentModel talentModel)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");

                if (talents.Exists(p => p.Name.ToLower() == talentModel.Name.ToLower()))
                {
                    Talent talent = FindAndUpdateTalentProperties(talentModel, talents);
                    talents.Update(talent);
                }
                else
                {
                    Talent talent = new Talent()
                    {
                        Name = talentModel.Name,
                        Description = talentModel.Description,
                        GameMechanics = talentModel.GameMechanics
                    };
                    talents.Insert(talent);
                }
            }
        }

        private static Talent FindAndUpdateTalentProperties(TalentModel talentModel, LiteCollection<Talent> talents)
        {
            var talent = talents.Find(m => m.Name == talentModel.Name).Single();
            talent.Name = talentModel.Name;
            talent.Description = talentModel.Description;
            talent.GameMechanics = talentModel.GameMechanics;
            return talent;
        }

        public Mutation GetMutation(string name)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var mutations = db.GetCollection<Mutation>("mutations");

                var result = mutations.Find(p => p.Name.ToLower().Contains(name.ToLower()));

                return result.FirstOrDefault();
            }
        }

        public void UpsertMutation(MutationModel mutationModel)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var mutations = db.GetCollection<Mutation>("mutations");

                if (mutations.Exists(p => p.Name.ToLower() == mutationModel.Name.ToLower()))
                {
                    Mutation mutation = FindAndUpdateMutationProperties(mutationModel, mutations);
                    mutations.Update(mutation);
                }
                else
                {
                    Mutation mutation = new Mutation()
                    {
                        Name = mutationModel.Name,
                        Description = mutationModel.Description,
                        GameMechanics = mutationModel.GameMechanics
                    };
                    mutations.Insert(mutation);
                }
            }
        }

        private static Mutation FindAndUpdateMutationProperties(MutationModel mutationModel, LiteCollection<Mutation> mutations)
        {
            var mutation = mutations.Find(m => m.Name == mutationModel.Name).Single();
            mutation.Name = mutationModel.Name;
            mutation.Description = mutationModel.Description;
            mutation.GameMechanics = mutationModel.GameMechanics;
            return mutation;
        }

        public Artifact GetArtifact(string name)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var artifacts = db.GetCollection<Artifact>("artifacts");

                var result = artifacts.Find(p => p.Name.ToLower().Contains(name.ToLower()));

                return result.FirstOrDefault();
            }
        }

        public void UpsertArtifact(ArtifactModel artifactModel)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var artifacts = db.GetCollection<Artifact>("artifacts");

                if (artifacts.Exists(p => p.Name.ToLower() == artifactModel.Name.ToLower()))
                {
                    Artifact artifact = FindAndUpdateArtifactProperties(artifactModel, artifacts);
                    artifacts.Update(artifact);
                }
                else
                {
                    Artifact artifact = new Artifact()
                    {
                        Name = artifactModel.Name,
                        Description = artifactModel.Description,
                        GameMechanics = artifactModel.GameMechanics
                    };
                    artifacts.Insert(artifact);
                }
            }
        }

        private static Artifact FindAndUpdateArtifactProperties(ArtifactModel artifactModel, LiteCollection<Artifact> artifacts)
        {
            var artifact = artifacts.Find(m => m.Name == artifactModel.Name).Single();
            artifact.Name = artifactModel.Name;
            artifact.Description = artifactModel.Description;
            artifact.GameMechanics = artifactModel.GameMechanics;
            return artifact;
        }

        public bool RemoveTalent(TalentModel talent)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");

                int result = talents.Delete(p => p.Id == talent.Id);

                if (result == 1)
                {
                    return true;
                }
                if (result == 0)
                {
                    return false;
                }
            }

            return false;
        }

        public bool RemoveMutation(MutationModel mutation)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var mutations = db.GetCollection<Mutation>("mutations");

                int result = mutations.Delete(p => p.Id == mutation.Id);

                if (result == 1)
                {
                    return true;
                }
                if (result == 0)
                {
                    return false;
                }
            }

            return false;
        }

        public bool RemoveArtifact(ArtifactModel artifact)
        {
            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var artifacts = db.GetCollection<Artifact>("artifacts");

                int result = artifacts.Delete(p => p.Id == artifact.Id);

                if (result == 1)
                {
                    return true;
                }
                if (result == 0)
                {
                    return false;
                }
            }

            return false;
        }

        public BindingList<ArtifactModel> GetAllArtifacts()
        {
            BindingList<ArtifactModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var artifacts = db.GetCollection<Artifact>("artifacts");
                var query = from a in artifacts.FindAll()
                            select new ArtifactModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };


                list = new BindingList<ArtifactModel>(query.ToList());
            }
            return list;
        }

        public BindingList<TalentModel> GetAllTalents()
        {
            BindingList<TalentModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");
                var query = from a in talents.FindAll()
                            select new TalentModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };

                list = new BindingList<TalentModel>(query.ToList());
            }
            return list;
        }

        public BindingList<MutationModel> GetAllMutations()
        {
            BindingList<MutationModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var mutations = db.GetCollection<Mutation>("mutations");
                var query = from a in mutations.FindAll()
                            select new MutationModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };

                list = new BindingList<MutationModel>(query.ToList());
            }
            return list;
        }

        public BindingList<ArtifactModel> GetArtifactsByNameSearch(string nameLike)
        {
            BindingList<ArtifactModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var artifacts = db.GetCollection<Artifact>("artifacts");
                var query = from a in artifacts.FindAll()
                            where a.Name.ToLower().Contains(nameLike.ToLower())
                            select new ArtifactModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };


                list = new BindingList<ArtifactModel>(query.ToList());
            }
            return list;
        }

        public BindingList<TalentModel> GetTalentsByNameSearch(string nameLike)
        {
            BindingList<TalentModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var talents = db.GetCollection<Talent>("talents");
                var query = from a in talents.FindAll()
                            where a.Name.ToLower().Contains(nameLike.ToLower())
                            select new TalentModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };

                list = new BindingList<TalentModel>(query.ToList());
            }
            return list;
        }

        public BindingList<MutationModel> GetMutationsByNameSearch(string nameLike)
        {
            BindingList<MutationModel> list = null;

            using (var db = new LiteDatabase(@"Mutant.db"))
            {
                var mutations = db.GetCollection<Mutation>("mutations");
                var query = from a in mutations.FindAll()
                            where a.Name.ToLower().Contains(nameLike.ToLower())
                            select new MutationModel
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Description = a.Description,
                                GameMechanics = a.GameMechanics
                            };

                list = new BindingList<MutationModel>(query.ToList());
            }
            return list;
        }
    }
}
