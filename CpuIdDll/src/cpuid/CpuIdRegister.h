#ifndef RJCP_DIAGNOSTICS_CPUID_CPUIDREGISTER_H
#define RJCP_DIAGNOSTICS_CPUID_CPUIDREGISTER_H

#include <cstdint>

namespace rjcp {
namespace diagnostics {
namespace cpuid {

/// <summary>
/// Contains the result of a CPUID instruction execution.
/// </summary>
/// <remarks>This class is generic enough to also represent CPUID information as obtained by a file.</remarks>
class CpuIdRegister {
 public:
  /// <summary>
  /// Initializes a new instance of the <see cref="CpuIdRegister"/> class with default values of zero and invalid.
  /// </summary>
  /// <remarks>The default constructor marks this object as invalid.</remarks>
  CpuIdRegister() noexcept;

  /// <summary>
  /// Initializes a new instance of the <see cref="CpuIdRegister"/> class from the result of a CPUID invocation.
  /// </summary>
  /// <param name="ieax">The input EAX register used.</param>
  /// <param name="iecx">The input ECX register used.</param>
  /// <param name="eax">The resultant EAX register.</param>
  /// <param name="ebx">The resultant EBX register.</param>
  /// <param name="ecx">The resultant ECX register.</param>
  /// <param name="edx">The resultant EDX register.</param>
  CpuIdRegister(std::uint32_t ieax, std::uint32_t iecx, std::uint32_t eax, std::uint32_t ebx, std::uint32_t ecx,
                std::uint32_t edx) noexcept;

  /// <summary>
  /// Returns if this instance has valid data.
  /// </summary>
  /// <returns>Returns true if contains valid data; false otherwise.</returns>
  auto IsValid() const noexcept -> bool;

  /// <summary>
  /// The input EAX register used.
  /// </summary>
  /// <returns>The input EAX register used.</returns>
  auto InEax() const noexcept -> std::uint32_t;

  /// <summary>
  /// The input ECX register used.
  /// </summary>
  /// <returns>The input ECX register used.</returns>
  auto InEcx() const noexcept -> std::uint32_t;

  /// <summary>
  /// The resultant EAX register after the CPUID instruction.
  /// </summary>
  /// <returns>The resultant EAX register used after the CPUID instruction.</returns>
  auto Eax() const noexcept -> std::uint32_t;

  /// <summary>
  /// The resultant EBX register after the CPUID instruction.
  /// </summary>
  /// <returns>The resultant EBX register used after the CPUID instruction.</returns>
  auto Ebx() const noexcept -> std::uint32_t;

  /// <summary>
  /// The resultant ECX register after the CPUID instruction.
  /// </summary>
  /// <returns>The resultant ECX register used after the CPUID instruction.</returns>
  auto Ecx() const noexcept -> std::uint32_t;

  /// <summary>
  /// The resultant EDX register after the CPUID instruction.
  /// </summary>
  /// <returns>The resultant EDX register used after the CPUID instruction.</returns>
  auto Edx() const noexcept -> std::uint32_t;

 private:
  std::uint32_t ineax_;
  std::uint32_t inecx_;
  std::uint32_t eax_;
  std::uint32_t ebx_;
  std::uint32_t ecx_;
  std::uint32_t edx_;
  bool isvalid_;
};

}  // namespace cpuid
}  // namespace diagnostics
}  // namespace rjcp
#endif
