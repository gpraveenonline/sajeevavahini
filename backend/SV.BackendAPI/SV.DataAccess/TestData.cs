using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using SV.Infrastructure;
using SV.DataAccess.Models;

namespace SV.DataAccess
{
    public class TestDAL
    {
        private string connectionString;

        private SqlDbRepository<Test> testRepository;

        public TestDAL(string connectionString)
        {
            this.connectionString = connectionString;

            if (testRepository == null)
                testRepository = new SqlDbRepository<Test>(connectionString);
        }

        public async Task<IEnumerable<Test>> GetTestList()
        {
            try
            {

                //Sample Logic to excute a select query or a view.
                var testList = await testRepository.ExecuteQueryAsync<Test>("SELECT * FROM [Test]", null);

                return testList;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Test> GetTestById(int Id)
        {
            try
            {
                //Sample Logic to excute a select query or a view to filter data.
                {
                    var qrySelectById = @"SELECT * FROM [Test] WHERE TestId=@Id";

                    var test = await testRepository.QuerySingleOrDefaultAsync<Test>(qrySelectById, new { Id });

                    return test;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Test>> GetTestBySp(int id)
        {
            try
            {
                //Sample Logic to excute a select query or a view using a stored procedure with parameters.
                {
                    var test = await testRepository.ExecuteSPQueryAsync<Test>("GetTestById", new { Id = id }, commandType: CommandType.StoredProcedure);

                    return test;
                }
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> AddTest(Test test)
        {
            try
            {
                //Sample Logic to insert data using Insert template
                var qrySelectById = @"INSERT INTO [Test](TestId, TestName)
                                      VALUES(@TestId, @TestName)";

                var addStatus = await testRepository.AddAsync(test);

                return (int)addStatus;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> UpdateTest(Test test)
        {
            try
            {
                //Sample Logic to Update records in database
                var updateStatus = await testRepository.UpdateAsync(test);

                return updateStatus ? 1 : -1;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        public async Task<int> DeleteTest(Test test)
        {
            try
            {
                //Sample Logic to delete data based on the parameters
                var deleteStatus = await testRepository.DeleteAsync(test);

                return deleteStatus ? 1 : -1;

            }
            catch (Exception ex)
            {
                return -1;
            }
        }

    }
}
